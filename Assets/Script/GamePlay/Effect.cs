using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// ÌØÐ§½Å±¾
    /// </summary>
    public class Effect : MonoBehaviour
    {

        public float animationTime;
        public string resourcePath;

        private void OnEnable()
        {
            Invoke("DestoryEffect", animationTime);
        }

        private void DestoryEffect()
        {
            GameController.Instance.PushGameObjectToFactory(resourcePath, gameObject);
        }
    }
}
