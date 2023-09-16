using UnityEngine.SceneManagement;


namespace Carrot
{
    public class StartLoadWindowState : WindowState
    {

        public StartLoadWindowState(UIFacade uIFacade): base(uIFacade) { }


        public override void EnterWindow()
        {
            UIFacade.AddPanelToDict(StringManager.StartLoadPanel);
            base.EnterWindow();          
        }

        public override void ExitWindow()
        {
            base.ExitWindow();
            SceneManager.LoadScene(1);
        }
    }
}
