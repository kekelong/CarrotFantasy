using System.Collections.Generic;
using UnityEngine;

namespace Carrot
{
    public class UIWindowManager
    {
        public UIFacade mUIFacade;
        public Dictionary<string, GameObject> currentWindowPanelDict;
        private GameManager mGameManager;

        public UIWindowManager()
        {
            mGameManager = GameManager.Instance;
            currentWindowPanelDict = new Dictionary<string, GameObject>();
            mUIFacade = new UIFacade(this);

            mUIFacade.currentWindowState = new StartLoadWindowState(mUIFacade);
        }

        //将UIPanel放回工厂
        private void PushUIPanel(string uiPanelName, GameObject uiPanelGo)
        {
            mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, uiPanelName, uiPanelGo);
        }

        //清空字典
        public void ClearDict()
        {
            foreach (var item in currentWindowPanelDict)
            {
                PushUIPanel(item.Value.name.Substring(0, item.Value.name.Length - 7), item.Value);
            }

            currentWindowPanelDict.Clear();
        }
    }
}
