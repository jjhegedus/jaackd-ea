﻿using System;
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

namespace JaackdEAAddin {
  internal class MDGService : IMDGService {
    private readonly ILogger<MDGService> _logger;
    string _mdgFile = string.Empty;
    string _mdgXML = string.Empty;
    XElement? _jaackdProfile;
    XElement? _jaackdDiagramProfile;
    XElement? _jaackdToolboxProfile;
    IEnumerable<XElement>? _stereotypes;

    internal MDGService(ILogger<MDGService> logger, string mdgFile) {
      _logger = logger;
      _mdgFile = mdgFile;
      _mdgXML = System.IO.File.ReadAllText(_mdgFile);
      processMDGXML(_mdgXML);
    }

    public string GetMDGXML() {
      return _mdgXML;
    }

    public IEnumerable<XElement>? GetStereotypes() {
      return _stereotypes;
    }

    private void LoadStereotypes() {
      if (_jaackdProfile != null) {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _stereotypes = _jaackdProfile.Descendants("Stereotype");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        foreach (XElement stereotype in _stereotypes) {
          XAttribute? stereotypeName = stereotype.Attribute("name");

          if (stereotypeName is not null) {
            _logger.LogInformation("found stereotype " + stereotypeName.Value);
          }
        }
      }
    }

    void processMDGXML(string mdgXML) {
      // Loading from a file, you can also load from a stream
      XElement parsedElement = XElement.Parse(mdgXML);
      GetProfilesFromMDGParsedElement(parsedElement);

      //MessageBox.Show("parsedElement = " + parsedElement.ToString());

      //XmlReader reader = XmlReader.Create(new StringReader(_mdgXML));
      //XElement root = XElement.Load(reader);
      //XmlNameTable nameTable = reader.NameTable;
      //XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
      //namespaceManager.AddNamespace("MDG", "");
      //IEnumerable<XElement> elements = root.XPathSelectElements("./aw:Child1", namespaceManager);
      //foreach (XElement el in elements)
      //    Console.WriteLine(el);


    }

    IEnumerable<XElement> GetProfilesFromMDGParsedElement(XElement parsedElement) {
      IEnumerable<XElement> profiles = parsedElement.Descendants("UMLProfile");
      foreach (XElement profile in profiles) {
        XElement? doc = profile.XPathSelectElement("./Documentation");
        if (doc is not null) {
          XAttribute docName = doc.Attribute("name");
          if (docName is not null) {
            if (docName.Value == "jaackd") {
              _logger.LogInformation("got the jaackd profile");
              _jaackdProfile = profile;
              LoadStereotypes();
            }

            if (docName.Value == "jaackd-diagram") {
              _logger.LogInformation("got the jaackd-diagram profile");
              _jaackdDiagramProfile = profile;
            }

            if (docName.Value == "jaackd-toolbox") {
              _logger.LogInformation("got the jaackd-toolbox profile");
              _jaackdToolboxProfile = profile;
            }
          }
        }
      }

      return profiles;
    }



  }
}