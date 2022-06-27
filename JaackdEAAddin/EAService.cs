using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using EA;

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

  }
}
