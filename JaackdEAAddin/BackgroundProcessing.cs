using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class BackgroundProcessing : IBackgroundProcessing {
    private readonly ILogger<BackgroundProcessing> _logger;
    private readonly IConfiguration _configuration;

    public BackgroundProcessing(ILogger<BackgroundProcessing> logger, IConfiguration configuration) {
      _logger = logger;
      _configuration = configuration;
    }

    public void Run() {
      int numTasks = _configuration.GetValue<int>("numTasks");
      for (int i = 0; i < numTasks; i++) {
        Task logActionTask = Task.Factory.StartNew(LogAction, new Tuple<string, ILogger<BackgroundProcessing>>(string.Format("Run number {0}", i), _logger));
      }
    }

    Action<object?> LogAction = (object? obj) => {

      (string message, ILogger<BackgroundProcessing> logger) = obj as Tuple<string, ILogger<BackgroundProcessing>>;

      logger.LogInformation("Message={0}; Task={1}; Thread={2}", message, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
    };

    //Action<object?> EAActivityAction = (object? obj) => {

    //};

  }
}
