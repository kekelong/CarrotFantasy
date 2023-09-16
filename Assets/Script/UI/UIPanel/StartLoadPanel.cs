using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carrot
{
    public class StartLoadPanel : BasePanel
    {

        protected override void Awake()
        {
            base.Awake();
            Invoke("LoadNextScene", 2);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadNextScene()
        {
            mUIFacade.ChangeSceneState(new MainWindowState(mUIFacade));
        }

    }
}

