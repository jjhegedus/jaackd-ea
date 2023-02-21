using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JaackdEAAddin {
  internal class MTSService : IMTSService {
    private readonly ILogger<MTSService> _logger;
    string _mtsFile = string.Empty;
    string _mtsXML = string.Empty;
    XElement? _parsedMTSElement;

    internal MTSService(ILogger<MTSService> logger, string mdgFile) {
      _logger = logger;
      _mtsFile = mdgFile;
      _mtsXML = System.IO.File.ReadAllText(_mtsFile);
      ProcessMTSXML(_mtsXML);
    }

    public string GetMTSXML() {
      throw new NotImplementedException();
    }

    void ProcessMTSXML(string mtsXML) {
      XElement parsedElement = XElement.Parse(mtsXML);
      InsertTemplates(parsedElement);
      _parsedMTSElement = parsedElement;
    }

    void InsertTemplates(XElement mdgParsedElement) {
      string templatesXML = GetTemplatesXML();

      XElement parsedTemplatesElement = XElement.Parse(templatesXML);
      XElement? mdgSelections = mdgParsedElement.XPathSelectElement("./MDG.Selections");
      if (mdgSelections != null) {
        mdgSelections.Add(parsedTemplatesElement);
      }

    }


    string GetTemplatesXML() {
      string templatesXML =
        "<ModelTemplates>\n" +
        "<Model \n" +
          "name=\"jaackd-patterns\" \n" +
          "location = \"c:\\p\\gh\\jjhegedus\\jaackd-ea\\jaackd-patterns.xml\"\n" +
          "default = \"yes\"\n" +
          "icon = \"34\"\n" +
          "isFramework = \"false\" />\n" +
        "</ModelTemplates > ";

      return templatesXML;
    }

  }
}
