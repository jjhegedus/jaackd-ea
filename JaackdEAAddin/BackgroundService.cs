using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class BackgroundService : IBackgroundService {
    private readonly ILogger<BackgroundService> _log;
    private readonly IConfiguration _configuration;

    public BackgroundService(ILogger<BackgroundService> log, IConfiguration configuration) {
      _log = log;
      _configuration = configuration;
    }

    public void Run() {
      for (int i = 0; i < _configuration.GetValue<int>("LoopTimes"); i++) {
        _log.LogInformation("Run number {runNUmber}", i);
      }
    }

  }
}
