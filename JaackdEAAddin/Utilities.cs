using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Serilog;

namespace JaackdEAAddin {
  internal class Utilities {

    public static IConfigurationBuilder BuildConfiguration(IConfigurationBuilder builder) {

      return builder.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
    }


    ///
    /// returns true if a project is currently opened
    ///
    /// <param name="Repository" />the repository
    /// true if a project is opened in EA
    public static bool IsProjectOpen(EA.Repository Repository) {
      try {
        EA.Collection c = Repository.Models;
        return true;
      } catch {
        return false;
      }
    }

  }
}
