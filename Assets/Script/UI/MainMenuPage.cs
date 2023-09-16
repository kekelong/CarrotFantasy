using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Carrot
{
    public class MainMenuPage : MonoBehaviour
    {
        public NormalModelpanel normalModelpanel;

        private void Awake()
        {
            
        }

        public void GoOn()
        {
            GameController.Instance.isPause = false;
            transform.gameObject.SetActive(false);
        }

        public void RePlaty()
        {
            normalModelpanel.RePlayGame();
        }

        public void OtherLevel()
        {
            normalModelpanel.SelectOtherLevel();
        }
    }
}
