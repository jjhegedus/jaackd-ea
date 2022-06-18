using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal class Utilities {

    public static void BuildConfiguration(IConfigurationBuilder builder) {
      builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
    }

    public static void Log(string message) {
      
    }




  }
}
