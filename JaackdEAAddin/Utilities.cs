using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Serilog;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.XPath;
using EA;

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

    public static IEnumerable<XElement> GetColumnXMLEnumerable(EA.Repository repository, string columnName, string tableName, string whereClause) {
      IEnumerable<XElement> result = new List<XElement>();
      string queryString = "SELECT " + columnName + " FROM " + tableName;
      if (whereClause.Length > 0) {
        queryString += " WHERE " + whereClause;
      }

      string queryResult = repository.SQLQuery(queryString);
      if (queryResult.Length > 0) {
        XElement parsedQueryResults = XElement.Parse(queryResult);
        result = parsedQueryResults.XPathSelectElements("//Data/Row/" + columnName);
      }

      return result;
    }

    //public static bool MoveToElement(EA.Repository repository, EA.Element element, EA.Element parentElement) {

    //  if (element != null && parentElement != null) {
    //    if (parentElement.Type == "Package") {
    //      return MoveToPackage(repository, element, repository.GetPackageByGuid(parentElement.ElementGUID));
    //    }

    //    //if (parentElement.Type == "Requirement") {
    //    //  if (element.Type == "Requirement") {
    //    element.ParentID = parentElement.ElementID;
    //    element.Update();
    //    return true;
    //    //  }
    //    //}
    //  }

    //  return false;
    //}

    //public static bool MoveToPackage(ref EA.Repository repository, ref EA.Element element, EA.Package parentPackage) {

    //  if (element != null && parentPackage != null) {
    //    if (element.Type == "Package") {
    //      Package elementPackage = repository.GetPackageByGuid(element.ElementGUID);
    //      elementPackage.PackageID = parentPackage.PackageID;
    //      elementPackage.Update();
    //    } else {
    //      element.PackageID = parentPackage.PackageID;
    //    }
    //    element.Update();
    //    return true;
    //  }

    //  return false;
    //}

  }
}
