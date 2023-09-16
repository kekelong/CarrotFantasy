using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carrot
{
    public class BasePanel : MonoBehaviour, IPanel
    {
        protected UIFacade mUIFacade;

        protected virtual void Awake()
        {
            mUIFacade = GameManager.Instance.UIWindowManager.mUIFacade;
        }

        public virtual void EnterPanel()
        {

        }

        public virtual void ExitPanel()
        {

        }

        public virtual void InitPanel()
        {

        }

        public virtual void UpdatePanel()
        {

        }



    }
}
