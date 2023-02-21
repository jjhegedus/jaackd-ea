using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class PatternService : IPatternService {
    private readonly ILogger<PatternService> _logger;
    string _patternFile = string.Empty;
    string _patternFileXML = string.Empty;

    internal PatternService(ILogger<PatternService> logger, string patternFile) {
      _logger = logger;
      _patternFile = patternFile;
      _patternFileXML = System.IO.File.ReadAllText(_patternFile);
    }

    public string GetPatternXML(EA.Repository repository, string location) {
      _patternFileXML = System.IO.File.ReadAllText(_patternFile);
      return _patternFileXML;
    }

  }
}
