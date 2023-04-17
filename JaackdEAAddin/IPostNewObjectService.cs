using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal interface IPostNewObjectService {
    bool EA_OnPostNewConnector(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewElement(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewDiagram(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewDiagramObject(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewAttribute(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewMethod(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewPackage(EA.Repository repository, EA.EventProperties eventProperties);

    bool EA_OnPostNewGlossaryTerm(EA.Repository repository, EA.EventProperties eventProperties);

  }
}
