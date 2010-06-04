using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace TimeFillets.Model
{
  public class ProjectSettingsConfigurationSectionHandler : IConfigurationSectionHandler
  {
    #region IConfigurationSectionHandler Members

    public object Create(object parent, object configContext, XmlNode section)
    {
      ProjectSettingsConfiguration configuration = new ProjectSettingsConfiguration();
      try
      {
        XElement sectionNode = XElement.Parse(section.OuterXml);
        XElement variablePaternNode = sectionNode.Elements().Where(element => element.Name == "variablePattern").First();
        XElement separatorNode = sectionNode.Elements().Where(element => element.Name == "separator").First();
        XElement patternNode = sectionNode.Elements().Where(element => element.Name == "pattern").First();
        XElement leftBracketNode = sectionNode.Elements().Where(element => element.Name == "leftBracket").First();
        XElement rightBracketNode = sectionNode.Elements().Where(element => element.Name == "rightBracket").First();
        XElement customerSignNode = sectionNode.Elements().Where(element => element.Name == "customerSign").First();
        XElement projectSignNode = sectionNode.Elements().Where(element => element.Name == "projectSign").First();
        XElement taskSingNode = sectionNode.Elements().Where(element => element.Name == "taskSign").First();

        configuration.VariablePattern = variablePaternNode.Value;
        configuration.Separator = separatorNode.Value;
        configuration.Pattern = patternNode.Value;
        configuration.LeftBracket = leftBracketNode.Value;
        configuration.RightBracket = rightBracketNode.Value;
        configuration.CustomerSign = customerSignNode.Value;
        configuration.ProjectSign = projectSignNode.Value;
        configuration.TaskSign = taskSingNode.Value;
      }
      catch (Exception e)
      {
        Trace.WriteLine(e.Message, "Error");
        throw e;
      }

      return configuration;
    }

    #endregion
  }
}
