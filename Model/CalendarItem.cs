using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TimeFillets.Model
{
  /// <summary>
  /// Class representing a event in calendar
  /// </summary>
  public class CalendarItem
  {
    /// <summary>
    /// Gets or sets a title of event
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a description of event
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Description cleaned of definition strings.
    /// </summary>
    public string CleanDescription
    {
      get
      {
        return ProjectSettings.ClearText(this.Description);
      }
    }

    /// <summary>
    /// Gets or sets a location of event
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Gets or sets a start date of event
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets a end date of event
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if event is repeating or not
    /// </summary>
    public bool IsRepeating { get; set; }

    /// <summary>
    /// Gets a events project item
    /// </summary>
    public Project ProjectItem { get; private set; }

    /// <summary>
    /// Gets events customer items
    /// </summary>
    public Customer CustomerItem { get; private set; }

    /// <summary>
    /// Gets events task item
    /// </summary>
    public Task TaskItem { get; private set; }

    /// <summary>
    /// Gets a duration of event
    /// </summary>
    public TimeSpan Duration
    {
      get { return this.EndDate - this.StartDate; }
      set { this.EndDate = StartDate + value; }
    }

    /// <summary>
    /// Get or set time when event was updated
    /// </summary>
    public DateTime Updated { get; set; }

    /// <summary>
    /// Googles event id
    /// </summary>
    public string GoogleEventId { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="customer">Events customer item</param>
    /// <param name="project">Events project item</param>
    /// <param name="task">Ecents task item</param>
    public CalendarItem(Customer customer, Project project, Task task) : this()
    {
      CustomerItem = customer;
      ProjectItem = project;
      TaskItem = task;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public CalendarItem()
    {
      this.StartDate = DateTime.Now;
      this.EndDate = DateTime.Now.AddHours(0.5);
      this.CustomerItem = new Customer();
      this.ProjectItem = new Project();
      this.TaskItem = new Task();
      this.Description = string.Empty;
    }

  }
}
