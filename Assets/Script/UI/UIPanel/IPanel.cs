using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Carrot
{
    public interface IPanel
    {
        void InitPanel();
        void EnterPanel();
        void ExitPanel();
        void UpdatePanel();

    }
}

