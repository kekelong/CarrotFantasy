using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Carrot
{
    public class MainWindowState : WindowState
    {
        public MainWindowState(UIFacade uIFacade) : base(uIFacade)
        {
        }
        public override void EnterWindow()
        {
            UIFacade.AddPanelToDict(StringManager.MainPanel);
            UIFacade.AddPanelToDict(StringManager.SetPanel);
            UIFacade.AddPanelToDict(StringManager.HelpPanel);
            UIFacade.AddPanelToDict(StringManager.GameLoadPanel);
            base.EnterWindow();
        }

        public override void ExitWindow()
        {
            base.ExitWindow();
            //当前对象的类 类型
            if (UIFacade.currentWindowState.GetType() == typeof(NormalGameOptionWindowState))
            {
                SceneManager.LoadScene(2);
            }
            else if (UIFacade.currentWindowState.GetType() == typeof(BossGameOptionWindowState))
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                SceneManager.LoadScene(6);
            }
        }
    }
}
