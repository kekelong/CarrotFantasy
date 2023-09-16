using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class WindowState : IWindowState
    {
        protected UIFacade UIFacade;


        public WindowState(UIFacade uIFacade)
        {
            UIFacade = uIFacade;
        }

        public virtual void EnterWindow()
        {
           UIFacade.InitDict();
        }

        public virtual void ExitWindow()
        {
            UIFacade.ClearDict();
        }
    }
}
