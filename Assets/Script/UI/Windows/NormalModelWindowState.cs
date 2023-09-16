using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Carrot
{
    public class NormalModelWindowState : WindowState
    {
        public NormalModelWindowState(UIFacade uIFacade) : base(uIFacade)
        {
        }

        public override void EnterWindow()
        {
            UIFacade.AddPanelToDict(StringManager.GameLoadPanel);
            UIFacade.AddPanelToDict(StringManager.NormalModelPanel);
            base.EnterWindow();
        }

        public override void ExitWindow()
        {
            base.ExitWindow();
        }
    }
}
