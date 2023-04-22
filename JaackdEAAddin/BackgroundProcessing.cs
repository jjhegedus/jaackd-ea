using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace JaackdEAAddin {
  internal class BackgroundProcessing : IBackgroundProcessing {
    private readonly ILogger<BackgroundProcessing> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEAService _eaService;
    private readonly IMDGService _mdgService;
    private static readonly double _period = 2000;
    private System.Timers.Timer _timer = new System.Timers.Timer(_period);

    public BackgroundProcessing(ILogger<BackgroundProcessing> logger, IConfiguration configuration, IEAService eAService, IMDGService mDGService) {
      _logger = logger;
      _configuration = configuration;
      _eaService = eAService;
      _mdgService = mDGService;
      _timer.AutoReset = true;
      _timer.Elapsed += OnTimedEvent;
    }

    public void Dispose() {
      _timer.Dispose();
    }

    public void Run() {
      _timer.Enabled = true;
    }

    public void Stop() {
      _timer.Enabled = false;
    }

    private void OnTimedEvent(Object? source, ElapsedEventArgs e) {
      _logger.LogInformation("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                        e.SignalTime);

      //foreach (EA.Connector jaackdConnector in _eaService.GetConnectorsBySterotypeName("jaackd-composition")) {
      //  _logger.LogInformation(jaackdConnector.FQStereotype);
      //  //ProcessJaackdCompositionConnector(jaackdConnector);
      //}

      //foreach (EA.Element jaackdElement in _eaService.GetElementsBySterotypeName("jaackd-problem-model")) {
      //  _logger.LogInformation(jaackdElement.FQStereotype);
      //}
    }

    private bool ProcessJaackdCompositionConnector(EA.Connector connector) {
      EA.Repository repository = _eaService.GetRepository();

      EA.Element clientElement = repository.GetElementByID(connector.ClientID);
      EA.Package clientPackage = repository.GetPackageByGuid(clientElement.ElementGUID);
      EA.Element supplierElement = repository.GetElementByID(connector.SupplierID);
      EA.Package supplierPackage = repository.GetPackageByGuid(supplierElement.ElementGUID);
      //EA.Package parentPackage = repository.GetPackageByID(clientElement.PackageID);

      if (clientPackage.ParentID != supplierPackage.PackageID) {
        //clientElement.PackageID = supplierPackage.PackageID;
        clientPackage.ParentID = supplierPackage.PackageID;
        clientPackage.Update();
        //supplierElement.Update();
        //parentPackage.Update();
      }


      return true;
    }

  }
}
