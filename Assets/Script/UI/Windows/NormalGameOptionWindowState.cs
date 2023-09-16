using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Carrot
{
    public class NormalGameOptionWindowState : WindowState
    {
        public NormalGameOptionWindowState(UIFacade uIFacade) : base(uIFacade)
        {
        }
        public override void EnterWindow()
        {
            UIFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
            UIFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
            UIFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
            UIFacade.AddPanelToDict(StringManager.HelpPanel);
            UIFacade.AddPanelToDict(StringManager.GameLoadPanel);
            base.EnterWindow();
        }

        public override void ExitWindow()
        {
            GameNormalOptionPanel gameNormalOptionPanel = UIFacade.currentWindowPanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
            if (gameNormalOptionPanel.isInBigLevelPanel)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(4);
            }
            gameNormalOptionPanel.isInBigLevelPanel = true;
            base.ExitWindow();
        }

    }
}
