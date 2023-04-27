using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.Logging;
using EA;
using Microsoft.Extensions.Configuration;

namespace JaackdEAAddin {
  internal class MDGService : IMDGService {
    private readonly ILogger<MDGService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMTSService _mtsService;
    string _mdgFile = string.Empty;
    string _mdgXML = string.Empty;
    XElement? _jaackdProfile;
    XElement? _jaackdDiagramProfile;
    XElement? _jaackdToolboxProfile;
    IEnumerable<XElement>? _stereotypesXML;
    XElement? _parsedElement;

    public MDGService(ILogger<MDGService> logger, IConfiguration configuration, IMTSService mtsService) {
      _logger = logger;
      _configuration = configuration;
      _mtsService = mtsService;
      _mdgFile = System.Environment.GetEnvironmentVariable("jaackd-ea") + _configuration.GetValue<string>("MDGFile");
      _mdgXML = System.IO.File.ReadAllText(_mdgFile);
      processMDGXML(_mdgXML);
    }

    public string GetMDGXML() {
      return _mdgXML;
    }

    public IEnumerable<XElement>? GetStereotypesXML() {
      return _stereotypesXML;
    }

    private void LoadStereotypes() {
      if (_jaackdProfile != null) {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _stereotypesXML = _jaackdProfile.Descendants("Stereotype");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        foreach (XElement stereotype in _stereotypesXML) {
          XAttribute? stereotypeName = stereotype.Attribute("name");

          if (stereotypeName is not null) {
            _logger.LogInformation("found stereotype " + stereotypeName.Value);
          }
        }
      }
    }

    void processMDGXML(string mdgXML) {
      // Loading from a file, you can also load from a stream
      _parsedElement = XElement.Parse(mdgXML);
      GetProfilesFromMDGParsedElement();
      _mtsService.InsertTemplates(ref _parsedElement);
      _parsedElement.Save(_mdgFile);
    }

    IEnumerable<XElement> GetProfilesFromMDGParsedElement() {
      IEnumerable<XElement> profiles = _parsedElement.Descendants("UMLProfile");
      foreach (XElement profile in profiles) {
        XElement? doc = profile.XPathSelectElement("./Documentation");
        if (doc is not null) {
          XAttribute docName = doc.Attribute("name");

          if (docName is not null) {

            if (docName.Value == "jaackd") {
              _logger.LogInformation("got the jaackd profile");
              _jaackdProfile = profile;
              LoadStereotypes();
            
            } else if (docName.Value == "jaackd-diagram") {
              _logger.LogInformation("got the jaackd-diagram profile");
              _jaackdDiagramProfile = profile;
            
            } else if (docName.Value == "jaackd-toolbox") {
              _logger.LogInformation("got the jaackd-toolbox profile");
              _jaackdToolboxProfile = profile;
            
            } else {
              _logger.LogInformation("got another profile named " + docName.Value);
            }

          }
        }
      }

      return profiles;
    }

    public void GenerateToolboxProfiles(Package package) {
      throw new NotImplementedException();
    }

    public void GenerateDiagramProfiles(Package package) {
      throw new NotImplementedException();
    }

    public void GenerateProfile(Package package) {
      throw new NotImplementedException();
    }

    public void GenerateMDG() {
      throw new NotImplementedException();
    }

  }
}
