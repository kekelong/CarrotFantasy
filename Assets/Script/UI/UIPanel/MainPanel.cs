using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Carrot
{
    public class MainPanel : BasePanel
    {
        [SerializeField]
        private Transform monsterTrans;

        [SerializeField]
        private Transform cloudTrans;


        private Tween[] mainPanelTween;//0.右，1.左
        private Tween ExitTween;//离开主页运行的动画


        protected override void Awake()
        {
            base.Awake();
            //获取成员变量
            transform.SetSiblingIndex(8);


            mainPanelTween = new Tween[2];

            mainPanelTween[0] = transform.DOLocalMoveX(1920, 0.5f);
            mainPanelTween[0].SetAutoKill(false);
            mainPanelTween[0].Pause();

            mainPanelTween[1] = transform.DOLocalMoveX(-1920, 0.5f);
            mainPanelTween[1].SetAutoKill(false);
            mainPanelTween[1].Pause();

            PlayUITween();
        }

        public override void EnterPanel()
        {
            transform.SetSiblingIndex(8);
            if (ExitTween != null)
            {
                ExitTween.PlayBackwards();
            }
            cloudTrans.gameObject.SetActive(true);
        }

        public override void ExitPanel()
        {
            ExitTween.PlayForward();
            cloudTrans.gameObject.SetActive(false);
        }

        //UI动画播放
        private void PlayUITween()
        {
            monsterTrans.DOLocalMoveY(-10, 1.5f).SetLoops(-1, LoopType.Yoyo);
            cloudTrans.DOLocalMoveX(1300, 8f).SetLoops(-1, LoopType.Yoyo);
        }

        public void MoveToRight()
        {
            ExitTween = mainPanelTween[0];
            mUIFacade.currentWindowPanelDict[StringManager.SetPanel].EnterPanel();
        }

        public void MoveToLeft()
        {
            ExitTween = mainPanelTween[1];
            mUIFacade.currentWindowPanelDict[StringManager.HelpPanel].EnterPanel();
        }

        //场景状态切换的方法

        public void ToNormalModelScene()
        {
            mUIFacade.currentWindowPanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new NormalGameOptionWindowState(mUIFacade));
        }

        public void ToBossModelScene()
        {
            mUIFacade.currentWindowPanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new BossGameOptionWindowState(mUIFacade));
        }

        public void ToMonsterNest()
        {
            mUIFacade.currentWindowPanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new MonsterNestWindowState(mUIFacade));
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}

