using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    public class TopMenuPage : MonoBehaviour
    {
        public TMP_Text Text_CoinCount;
        public TMP_Text Text_RoundCount;
        public TMP_Text Text_TotalCount;
        public Image Image_gameSpeed;
        public Image Image_gamePause;
        public GameObject OnPause;
        public GameObject Text_Num;
        public NormalModelpanel normalModelpanel;
        // Start is called before the first frame update

        public Sprite[] gameSpeedSprite;
        public Sprite[] gamepauseSprite;


        private bool isNurmalSpeed;
        private bool isPause;


        private void Awake()
        {
            isNurmalSpeed = true;
        }

        private void OnEnable()
        { 
            Image_gamePause.sprite = gamepauseSprite[0];
            Image_gameSpeed.sprite = gameSpeedSprite[0];
            isPause = false;
            isNurmalSpeed = true;
            OnPause.SetActive(false);
            Text_Num.SetActive(true);
        }

        public void UpdateCoinText()
        {
            if (Text_CoinCount != null) 
            {
                Text_CoinCount.text = GameController.Instance.coin.ToString();
            }
            else
            {
                Debug.Log("Text_CoinCount is null");
            }
            
        }
        public void UpdatRoundText()
        {
            normalModelpanel.ShowRoundText(Text_RoundCount);
        }

        /// <summary>
        /// 改变速度
        /// </summary>
        /// 
        public void ChageGameSpeed()
        {
            isNurmalSpeed = !isNurmalSpeed;
            if(isNurmalSpeed)
            {
                GameController.Instance.gameSpeed = 1;
                Image_gameSpeed.sprite = gameSpeedSprite[0];
            }
            else{
                GameController.Instance.gameSpeed = 2;
                Image_gameSpeed.sprite = gameSpeedSprite[1];
            }
        }

        public void PauseGame()
        {
            isPause = !isPause;

            if (isPause)
            {
                GameController.Instance.isPause = true;
                Image_gamePause.sprite = gamepauseSprite[1];
                OnPause.SetActive(true);
                Text_Num.SetActive(false );
            }
            else
            {
                GameController.Instance.isPause = false;
                Image_gamePause.sprite = gamepauseSprite[0];
                OnPause.SetActive(false);
                Text_Num.SetActive(true);
            }
        }


        public void ShowMenu()
        {
            normalModelpanel.ShowMainMenuPage();
        }
    }
}

