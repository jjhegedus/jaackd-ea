using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class PatternService : IPatternService {
    private readonly ILogger<PatternService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMTSService _mtsService;
    string _baseJaackdFolder = System.Environment.GetEnvironmentVariable("jaackd-ea");
    string _patternsFolder = string.Empty;
    Dictionary<string, string> _patterns = new Dictionary<string, string>();

    public PatternService(ILogger<PatternService> logger, IConfiguration configuration, IMTSService mtsService) {
      _logger = logger;
      _configuration = configuration;
      _mtsService = mtsService;
      _patternsFolder = _baseJaackdFolder + _configuration.GetValue<string>("PatternsFolder");
      LoadPatterns();
    }

    private void LoadPatterns() {

      DirectoryInfo directoryInfo = new DirectoryInfo(_patternsFolder);
      FileInfo[] files = directoryInfo.GetFiles();


      foreach (FileInfo file in files) {
        _patterns.Add(Path.GetFileNameWithoutExtension(file.FullName), System.IO.File.ReadAllText(file.FullName));
      }

    }

    public string GetPatternXML(EA.Repository repository, string location) {
      //string patternFileName = _patternsFolder + "\\" + location + "-pattern.xml";
      //string patternFileXML = System.IO.File.ReadAllText(patternFileName);
      //return patternFileXML;

      return _patterns[location];
    }

  }
}
