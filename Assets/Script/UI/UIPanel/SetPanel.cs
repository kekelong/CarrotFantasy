using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Carrot
{
    public class SetPanel : BasePanel
    {
        [SerializeField]
        private GameObject OptionPage;

        [SerializeField]
        private GameObject DataPage;

        [SerializeField]
        private GameObject AuthorPage;

        [SerializeField]
        private Image Btn_EffectAudio;

        [SerializeField]
        private Image Btn_BGAudio;
        private Tween setPanelTween;
        private bool playBGMusic = true;
        private bool playEffectMusic = true;
        public Sprite[] btnSprites;//0.音效开 1.音效关 2.背景音乐开 3.背景音乐关

        public TMP_Text[] statisticesTexts;

        protected override void Awake()
        {
            base.Awake();
            setPanelTween = transform.DOLocalMoveX(0, 0.5f);
            setPanelTween.SetAutoKill(false);
            setPanelTween.Pause();
        }

        public override void InitPanel()
        {
            transform.localPosition = new Vector3(-1920, 0, 0);
            transform.SetSiblingIndex(2);
        }

        //显示页面的方法
        public void ShowOptionPage()
        {
            OptionPage.SetActive(true);
            DataPage.SetActive(false);
            AuthorPage.SetActive(false);
        }

        public void ShowStatisticsPage()
        {
            OptionPage.SetActive(false);
            DataPage.SetActive(true);
            AuthorPage.SetActive(false);
            ShowStatistics();
        }

        public void ShowProducerPage()
        {
            OptionPage.SetActive(false);
            DataPage.SetActive(false);
            AuthorPage.SetActive(true);
        }

        //进入退出页面的方法
        public override void EnterPanel()
        {
            ShowOptionPage();
            MoveToCenter();
        }

        public override void ExitPanel()
        {
            setPanelTween.PlayBackwards();
            mUIFacade.currentWindowPanelDict[StringManager.MainPanel].EnterPanel();
            InitPanel();
        }


        public void MoveToCenter()
        {
            setPanelTween.PlayForward();
        }

        /// <summary>
        /// 音乐处理
        /// </summary>
        public void CloseOrOpenBGMusic()
        {
            playBGMusic = !playBGMusic;
            mUIFacade.CloseOrOpenBGMusic();
            if (playBGMusic)
            {
                Btn_BGAudio.sprite = btnSprites[2];
            }
            else
            {
                Btn_BGAudio.sprite = btnSprites[3];
            }
        }

        public void CloseOrOpenEffectMusic()
        {
            playEffectMusic = !playEffectMusic;
            mUIFacade.CloseOrOpenEffectMusic();
            if (playEffectMusic)
            {
                Btn_EffectAudio.sprite = btnSprites[0];
            }
            else
            {
                Btn_EffectAudio.sprite = btnSprites[1];
            }
        }

        //数据显示
        public void ShowStatistics()
        {
            PlayerManager playerManager = mUIFacade.mPlayerManager;
            statisticesTexts[0].text = playerManager.adventrueModelNum.ToString();
            statisticesTexts[1].text = playerManager.burriedLevelNum.ToString();
            statisticesTexts[2].text = playerManager.bossModelNum.ToString();
            statisticesTexts[3].text = playerManager.coin.ToString();
            statisticesTexts[4].text = playerManager.killMonsterNum.ToString();
            statisticesTexts[5].text = playerManager.killBossNum.ToString();
            statisticesTexts[6].text = playerManager.clearItemNum.ToString();
        }

        //重置游戏
        public void ResetGame()
        {
            GameManager.Instance.initPlayManager = true;
            GameManager.Instance.PlayerManager.ReadData();
            ShowStatistics();
        }

    }
}

