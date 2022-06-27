using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaackdEAAddin {
  internal interface IMenuService {
    object GetMenuItems(EA.Repository Repository, string Location, string MenuName);
    void GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked);
    public void MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName);

  }
}
