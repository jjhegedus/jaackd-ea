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

    public static Dictionary<string, Tuple<Type, string>> GetInput(string formName, Dictionary<string, Type> dataElements) {
      Dictionary<string, Tuple<Type, string>> results = new Dictionary<string, Tuple<Type, string>>();
      Dictionary<string, Tuple<Control, Control>> controls = new Dictionary<string, Tuple<Control, Control>>();

      const int topMargin = 30;
      const int rowSize = 13;
      const int rowMargin = 15;
      const int buttonMargin = 30;
      const int buttonHeight = 60;
      const int buttonWidth = 160;

      int formHeight = topMargin;

      Form form = new Form();
      form.Text = formName;
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.MinimizeBox = false;
      form.MaximizeBox = false;

      int iteration = 0;
      foreach (string key in dataElements.Keys) {

        Label lable = new Label();
        lable.Text = key;
        lable.SetBounds(36, 36 + (iteration * (rowSize + rowMargin)), 60, 13);
        lable.AutoSize = true;
        TextBox textBox = new TextBox();
        textBox.SetBounds(150, 36 + (iteration * (rowSize + rowMargin)), 300, 13);
        form.Controls.AddRange(new Control[] { lable, textBox });

        controls[key] = Tuple.Create((Control)lable, (Control)textBox);
        iteration++;

        formHeight += rowSize + rowMargin;
      }

      formHeight += buttonMargin;

      Button buttonOk = new Button();
      buttonOk.Text = "OK";
      buttonOk.DialogResult = DialogResult.OK;
      buttonOk.SetBounds(228, formHeight, buttonWidth, buttonHeight);

      Button buttonCancel = new Button();
      buttonCancel.Text = "Cancel";
      buttonCancel.DialogResult = DialogResult.Cancel;
      buttonCancel.SetBounds(400, formHeight, buttonWidth, buttonHeight);

      form.Controls.AddRange(new Control[] { buttonOk, buttonCancel });
      form.AcceptButton = buttonOk;
      form.CancelButton = buttonCancel;

      formHeight += buttonHeight + buttonMargin;
      form.ClientSize = new System.Drawing.Size(796, formHeight);

      DialogResult dialogResult = form.ShowDialog();

      if (dialogResult == DialogResult.OK) {
        foreach (string key in controls.Keys) {
          results[key] = Tuple.Create(dataElements[key], controls[key].Item2.Text);
        }
      } 

      return results;
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
