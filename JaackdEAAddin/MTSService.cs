using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JaackdEAAddin
{
  internal class MTSService : IMTSService
  {
    private readonly ILogger<MTSService> _logger;
    private readonly IConfiguration _configuration;
    string _mtsFile = string.Empty;
    string _mtsXML = string.Empty;
    XElement? _parsedMTSElement;
    IEnumerable<XElement>? _templates;
    string _baseJaackdFolder = String.Empty;
    string _patternsFolder = String.Empty;

    public MTSService(ILogger<MTSService> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
      _baseJaackdFolder = System.Environment.GetEnvironmentVariable("jaackd-ea");
      _mtsFile = _baseJaackdFolder + _configuration.GetValue<string>("MTSFile");
      _patternsFolder = _baseJaackdFolder + _configuration.GetValue<string>("PatternsFolder");
      _mtsXML = System.IO.File.ReadAllText(_mtsFile);
      ProcessMTSXML(_mtsXML);
    }

    public string GetMTSXML()
    {
      throw new NotImplementedException();
    }


    public string GetTemplatesXML()
    {
      StringBuilder stringBuilder = new StringBuilder();

      DirectoryInfo directoryInfo = new DirectoryInfo(_patternsFolder);
      FileInfo[] files = directoryInfo.GetFiles();

      stringBuilder.Append(
        "<ModelTemplates>\n");

      foreach (FileInfo file in files)
      {

        stringBuilder.Append("<Model \n" +
          "name=\"" + Path.GetFileNameWithoutExtension(file.FullName) + "\" \n" +
          "location = \"" + file.FullName + "\"\n" +
          "default = \"yes\"\n" +
          "icon = \"34\"\n" +
          "isFramework = \"false\" />\n");
      }

      stringBuilder.Append("</ModelTemplates > ");

      string test = stringBuilder.ToString();

      return stringBuilder.ToString();
    }

    void LoadTemplates(ref XElement element)
    {
      string templatesXML = GetTemplatesXML();

      XElement parsedTemplatesElement = XElement.Parse(templatesXML);
      if (parsedTemplatesElement != null)
      {
        _templates = parsedTemplatesElement.Descendants("Model");
      }
    }

    public void InsertTemplates(ref XElement mdgParsedElement)
    {
      XElement? mdgSelections = mdgParsedElement.XPathSelectElement(".");
      if (mdgSelections != null)
      {
        XElement modelTemplatesNode = mdgSelections.XPathSelectElement(".//ModelTemplates");
        if(modelTemplatesNode != null)
        {
          modelTemplatesNode.Descendants().Remove();
          modelTemplatesNode.Add(GetTemplates());
        }
      }

    }

    public IEnumerable<XElement>? GetTemplates()
    {
      return _templates;
    }

    void ProcessMTSXML(string mtsXML)
    {
      XElement parsedElement = XElement.Parse(mtsXML);
      if(parsedElement != null)
      {
        LoadTemplates(ref parsedElement);
        InsertTemplates(ref parsedElement);
        _parsedMTSElement = parsedElement;
        _parsedMTSElement.Save(_mtsFile);
      }
    }

  }
}
