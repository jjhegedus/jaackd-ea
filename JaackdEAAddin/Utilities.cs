using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class Utilities {

    public static void BuildConfiguration(IConfigurationBuilder builder) {
      string directory = Directory.GetCurrentDirectory();
      builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
    }

  }
}
