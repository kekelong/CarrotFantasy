using System.Collections;
using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// 游戏物体工厂的接口
    /// </summary>
    public interface IFacotry
    {
        GameObject GetItem(string itemName);

        void PushItem(string itemName, GameObject item);
    }
}

