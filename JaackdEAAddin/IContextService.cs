using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin
{
    internal interface IContextService
    {
        public void OnContextItemChanged(EA.Repository Repository, string GUID, EA.ObjectType objectType);

        public bool OnContextItemDoubleClicked(EA.Repository Repository, string GUID, EA.ObjectType ot);

        public void OnNotifyContextItemModified(EA.Repository Repository, string GUID, EA.ObjectType ot);
    }
}
