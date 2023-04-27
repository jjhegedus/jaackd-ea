using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JaackdEAAddin
{
  internal interface IMTSService
  {
    string GetMTSXML();
    string GetTemplatesXML();
    IEnumerable<XElement>? GetTemplates();
    void InsertTemplates(ref XElement mdgParsedElement);
  }
}
