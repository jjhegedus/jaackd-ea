using System.Collections.Generic;

namespace JaackdEAAddin {
  internal interface IEAService {
    EA.Repository GetRepository();
    string GetEventPropertiesString(EA.EventProperties eventProperties);
    IEnumerable<EA.Element> GetElementsBySterotypeName(string stereotypeFQN);
    IEnumerable<EA.Connector> GetConnectorsBySterotypeName(string stereotypeFQN);
    EA.Stereotype GetStereotype(string stereotypeFQN);
    IEnumerable<EA.Connector> ConvertConnectorsFromStereotype(string fromStereotype, string toStereotype);
    IEnumerable<EA.Element> ConvertElementsFromStereotype(string fromStereotype, string toStereotype);
    EA.Element UpdateElementStereotype(EA.Element element, string toSterotype);
  }
}
