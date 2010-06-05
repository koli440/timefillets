using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TimeFillets.Model;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Xml;
using System.Configuration;

namespace TimeFillets.Connectors
{
  /// <summary>
  /// Provides functionality for project definitions
  /// </summary>
  public class ProjectDefinitionsConnector
  {
    private List<Customer> scannedItems;
    private readonly string projectDefinitionsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + ConfigurationManager.AppSettings["ProjectDefinitionPath"];

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProjectDefinitionsConnector()
    {
      scannedItems = new List<Customer>();
    }

    /// <summary>
    /// Scans calendar for project definitions
    /// </summary>
    /// <returns></returns>
    public XDocument ScanForDefinitions()
    {
      return ScanForDefinitions(DateTime.MinValue, DateTime.MaxValue);
    }

    /// <summary>
    /// Scans calendar for project definitions in given time range
    /// </summary>
    /// <param name="from">Begin time for search</param>
    /// <param name="to">End time for search</param>
    /// <returns></returns>
    public XDocument ScanForDefinitions(DateTime from, DateTime to)
    {
      try
      {
        if (!SettingsConnector.ApplicationSettings.IsConfigurationValid)
          throw new Exception("Application setting is not valid");
        GoogleCalendarConnector connector = new GoogleCalendarConnector(SettingsConnector.ApplicationSettings.UserName, SettingsConnector.ApplicationSettings.Password, SettingsConnector.ApplicationSettings.CalendarUrl, SettingsConnector.ApplicationSettings.ApplicationName);

        IEnumerable<CalendarItem> calendarItems = connector.GetCalendarItems(from, to);

        XDocument doc = new XDocument();

        foreach (CalendarItem item in calendarItems)
        {
          ensureCustomer(item.CustomerItem);
          ensureProject(item.ProjectItem, item.CustomerItem);
          ensureTask(item.TaskItem, item.ProjectItem, item.CustomerItem);
        }

        XElement rootElement = new XElement("projectsDefinitions");
        doc.Add(rootElement);

        foreach (Customer customer in scannedItems)
        {
          XElement customerElement = new XElement("customer", new XAttribute("name", customer.Name));
          doc.Root.Add(customerElement);
          foreach (Project project in customer.Projects)
          {
            XElement projectElement = new XElement("project", new XAttribute("name", project.Name));
            customerElement.Add(projectElement);
            foreach (Task task in project.Tasks)
            {
              XElement taskElement = new XElement("task", new XAttribute("name", task.Name));
              projectElement.Add(taskElement);
            }
          }
        }

        return doc;
      }
      catch (Exception e)
      {
        Trace.WriteLine(e.Message);
        throw;
      }
    }

    /// <summary>
    /// Saves definition file to program directory
    /// </summary>
    /// <param name="doc">Xml with project definitions</param>
    public void SaveDefinitions(XDocument doc)
    {
      using (FileStream definitionFile = new FileStream(projectDefinitionsFilePath, FileMode.Create, FileAccess.Write, FileShare.Write, 8))
      {
        using (StreamWriter writer = new StreamWriter(definitionFile))
        {
          writer.Write(doc.ToString());
        }
      }
    }

    /// <summary>
    /// Opens file with definition and returns them as <see cref="System.Xml.Linq.XDocument"/>
    /// </summary>
    /// <returns></returns>
    public XDocument OpenDefinitions()
    {
      XDocument document;
      using (FileStream definitionFile = new FileStream(projectDefinitionsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, 8))
      {
        try
        {
          document = XDocument.Load(definitionFile);
        }
        catch (XmlException e)
        {
          Trace.WriteLine(e);
          document = XDocument.Parse("<?xml version=\"1.0\" encoding=\"utf-8\" ?><projectsDefinitions></projectsDefinitions>"); ;
        }
      }
      return document;
    }

    /// <summary>
    /// Gets typed definitions
    /// </summary>
    /// <returns></returns>
    public ProjectsDefinitions GetDefinitions()
    {
      var doc = OpenDefinitions();
      var customers = GetCustomersFromXml(doc);

      ProjectsDefinitions ret = new ProjectsDefinitions(customers);
      return ret;
    }

    /// <summary>
    /// Ensures that customer exists in scanned items
    /// </summary>
    /// <param name="customer"></param>
    private void ensureCustomer(Customer customer)
    {
      if (!string.IsNullOrEmpty(customer.Name))
      {
        if (scannedItems == null)
          scannedItems = new List<Customer>();
        if (scannedItems.Where(item => item.Name == customer.Name).Count() == 0)
        {
          scannedItems.Add(customer);
        }
      }
    }

    /// <summary>
    /// Ensures that project exists in scanned items
    /// </summary>
    /// <param name="project"></param>
    /// <param name="customer"></param>
    private void ensureProject(Project project, Customer customer)
    {
      if (!string.IsNullOrEmpty(project.Name))
      {
        foreach (Customer savedCustomer in scannedItems)
        {
          if (savedCustomer.Name == customer.Name)
          {
            if (savedCustomer.Projects == null)
              savedCustomer.Projects = new List<Project>();
            if (savedCustomer.Projects.Where(p => p.Name == project.Name && p.Owner.Name == customer.Name).Count() == 0)
            {
              savedCustomer.Projects.Add(new Project() { Name = project.Name, Owner = savedCustomer });
            }
          }
        }
      }
    }

    /// <summary>
    /// Ensures that task exists in scanned items
    /// </summary>
    /// <param name="task"></param>
    /// <param name="project"></param>
    /// <param name="customer"></param>
    private void ensureTask(Task task, Project project, Customer customer)
    {
      if (!string.IsNullOrEmpty(task.Name))
      {
        foreach (Customer savedCustomer in scannedItems)
        {
          foreach (Project savedProject in savedCustomer.Projects)
          {
            if (savedProject.Name == project.Name && savedCustomer.Name == customer.Name)
            {
              if (savedProject.Tasks == null)
                savedProject.Tasks = new List<Task>();
              if (savedProject.Tasks.Where(t => t.Name == task.Name && t.Owner.Name == project.Name && t.Owner.Owner.Name == customer.Name).Count() == 0)
              {
                savedProject.Tasks.Add(new Task() { Name = task.Name, Owner = savedProject });
              }
            }
          }
        }
      }
    }

    private List<Project> GetProjectsFromXml(XElement customerElement, Customer customer)
    {
      List<Project> projects = new List<Project>();

      IEnumerable projectElements = customerElement.Elements().Where(element => element.Name == "project");

      foreach (XElement projectElement in projectElements)
      {
        Project project = new Project();
        project.Name = projectElement.Attributes().Where(attribute => attribute.Name == "name").First().Value;
        project.Owner = customer;
        project.Tasks = GetTasksFromXml(projectElement, project);
        projects.Add(project);
      }

      return projects;
    }

    private List<Task> GetTasksFromXml(XElement projectElement, Project project)
    {
      List<Task> tasks = new List<Task>();

      IEnumerable taskElements = projectElement.Elements().Where(element => element.Name == "task");

      foreach (XElement taskElement in taskElements)
      {
        Task task = new Task();
        task.Name = taskElement.Attributes().Where(attribute => attribute.Name == "name").First().Value;
        task.Owner = project;
        tasks.Add(task);
      }

      return tasks;
    }

    private List<Customer> GetCustomersFromXml(XDocument document)
    {
      IEnumerable customerElements = document.Root.Elements().Where(element => element.Name == "customer");

      List<Customer> customers = new List<Customer>();


      foreach (XElement element in customerElements)
      {
        Customer customer = new Customer();
        customer.Name = element.Attributes().Where(attribute => attribute.Name == "name").First().Value;
        customer.Projects = GetProjectsFromXml(element, customer);
        customers.Add(customer);
      }
      return customers;
    }

  }
}
