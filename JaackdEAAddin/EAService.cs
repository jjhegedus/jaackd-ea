using EA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JaackdEAAddin {
  internal class EAService : IEAService {

    private readonly ILogger<EAService> _logger;
    private readonly IConfiguration _configuration;
    private EA.Repository _repository;

    public EAService(ILogger<EAService> logger, IConfiguration configuration, EA.Repository repository) {
      _logger = logger;
      _configuration = configuration;
      _repository = repository;
    }

    public EA.Repository GetRepository() {
      return _repository;
    }


    public string GetEventPropertiesString(EA.EventProperties eventProperties) {

      StringBuilder builder = new StringBuilder();
      builder.Append("Event Properties:\n");

      foreach (EventProperty property in eventProperties) {
        builder.Append("  ");
        builder.Append(property.Name);
        builder.Append(" = ");
        builder.Append(property.Value);
        builder.Append("\n");
      }

      return builder.ToString();
    }


    public IEnumerable<Element> GetElementsBySterotypeName(string stereotypeFQN) {
      List<Element> elements = new List<Element>();

      IEnumerable<XElement> objectIdsXmlElements = Utilities.GetColumnXMLEnumerable(_repository, "Object_ID", "t_object", "Stereotype = '" + stereotypeFQN + "'");

      foreach (XElement xElement in objectIdsXmlElements) {
        int objectId = int.Parse(xElement.Value);
        elements.Add(_repository.GetElementByID(objectId));
      }

      return elements;
    }

    public Stereotype GetStereotype(string stereotypeFQN) {
      throw new System.NotImplementedException();
    }

    public IEnumerable<Connector> GetConnectorsBySterotypeName(string stereotypeFQN) {
      List<Connector> connectors = new List<Connector>();

      IEnumerable<XElement> connectorIdsXmlElements = Utilities.GetColumnXMLEnumerable(_repository, "Connector_ID", "t_connector", "Stereotype = '" + stereotypeFQN + "'");

      foreach (XElement xElement in connectorIdsXmlElements) {
        int connectorId = int.Parse(xElement.Value);
        connectors.Add(_repository.GetConnectorByID(connectorId));
      }

      return connectors;
    }
  }
}
