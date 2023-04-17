using EA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class PostNewObjectService : IPostNewObjectService {
    private readonly ILogger<BackgroundProcessing> _logger;
    private readonly IConfiguration _configuration;

    public PostNewObjectService(ILogger<BackgroundProcessing> logger, IConfiguration configuration) {
      _logger = logger;
      _configuration = configuration;
    }

    public bool EA_OnPostNewConnector(EA.Repository repository, EA.EventProperties eventProperties) {
      int connectorId = int.Parse((string)eventProperties.Get(0).Value);
      EA.Connector connector = repository.GetConnectorByID(connectorId);

      if (connector.FQStereotype == "jaackd::jaackd-composition") {

        Element clientElement = repository.GetElementByID(connector.ClientID);
        Package clientPackage = repository.GetPackageByGuid(clientElement.ElementGUID);
        Element supplierElement = repository.GetElementByID(connector.SupplierID);
        Package supplierPackage = repository.GetPackageByGuid(supplierElement.ElementGUID);

        if (supplierPackage != null) {
          if (clientPackage != null) {
            clientPackage.ParentID = supplierPackage.PackageID;
            clientPackage.Update();
          } else {
            clientElement.PackageID = supplierPackage.PackageID;
            clientElement.Update();
          }
        } else {
          clientElement.ParentID = supplierElement.ElementID;
          clientElement.Update();
        }



      }

      return true;
    }

    public bool EA_OnPostNewElement(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewElement");
      return true;
    }

    public bool EA_OnPostNewDiagram(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewDiagram");
      return true;
    }

    public bool EA_OnPostNewDiagramObject(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewDiagramObject");
      return true;
    }

    public bool EA_OnPostNewAttribute(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewAttribute");
      return true;
    }

    public bool EA_OnPostNewMethod(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewMethod");
      return true;
    }

    public bool EA_OnPostNewPackage(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewPackage");
      return true;
    }

    public bool EA_OnPostNewGlossaryTerm(EA.Repository repository, EA.EventProperties eventProperties) {
      _logger.LogInformation("EA_OnPostNewGlossaryTerm");
      return true;
    }
  }
}
