using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    public class CoinMove : MonoBehaviour
    {

        private TMP_Text coinText;
        private Image coinImage;
        public Sprite[] coinSprites;
        [HideInInspector]
        public int prize;

        private void Awake()
        {
            coinText = transform.Find("Coin_Num").GetComponent<TMP_Text>();
            coinImage = transform.Find("Image").GetComponent<Image>();
            coinSprites = new Sprite[2];
            coinSprites[0] = GameController.Instance.GetSprite("NormalMordel/Game/Coin");
            coinSprites[1] = GameController.Instance.GetSprite("NormalMordel/Game/ManyCoin");
        }

        private void OnEnable()
        {
            ShowCoin();
        }

        private void ShowCoin()
        {
            coinText.text = prize.ToString();
            //Í¼Æ¬ÏÔÊ¾
            if (prize >= 500)
            {
                coinImage.sprite = coinSprites[1];
            }
            else
            {
                coinImage.sprite = coinSprites[0];
            }
            DOTween.To(() => coinImage.color, toColor => coinImage.color = toColor, new Color(1, 1, 1, 0), 0.7f);
            Tween tween = DOTween.To(() => coinText.color, toColor => coinText.color = toColor, new Color(1, 1, 1, 0), 0.7f);
            tween.OnComplete(DestoryCoin);
        }

        //Ïú»Ù½ð±ÒUI
        private void DestoryCoin()
        {
            transform.localPosition = Vector3.zero;
            coinImage.color = coinText.color = new Color(1, 1, 1, 1);
            GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
        }
    }
}

