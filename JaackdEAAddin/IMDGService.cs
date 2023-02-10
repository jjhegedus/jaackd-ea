using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JaackdEAAddin
{
    internal interface IMDGService
    {
        string GetMDGXML();
        IEnumerable<XElement>? GetStereotypes();
    }
}
