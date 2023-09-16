using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Carrot
{
    public class HelpPanel : BasePanel
    {
        [SerializeField]
        private GameObject helpPageGo;

        [SerializeField]
        private GameObject monsterPageGo;

        [SerializeField]
        private GameObject towerPageGo;

        [SerializeField]
        private SlideCanCoverScrollView helpPageSlideScrollView;

        [SerializeField]
        private SlideCanCoverScrollView towerPageSlideScrollView;

        private Tween helpPanelTween;


        protected override void Awake()
        {
            base.Awake();
            helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
            helpPanelTween.SetAutoKill(false);
            helpPanelTween.Pause();

        }

        //显示页面的方法
        public void ShowHelpPage()
        {
            if (!helpPageGo.activeSelf)
            {
                helpPageGo.SetActive(true);
            }
            monsterPageGo.SetActive(false);
            towerPageGo.SetActive(false);
        }

        public void ShowMonsterPage()
        {
            helpPageGo.SetActive(false);
            monsterPageGo.SetActive(true);
            towerPageGo.SetActive(false);
        }

        public void ShowTowerPage()
        {
            helpPageGo.SetActive(false);
            monsterPageGo.SetActive(false);
            towerPageGo.SetActive(true);
        }

        //处理面板的方法

        public override void InitPanel()
        {
            base.InitPanel();

            transform.SetSiblingIndex(5);
            helpPageSlideScrollView.Init();
            towerPageSlideScrollView.Init();
            ShowHelpPage();

            //其他处理
            if (transform.localPosition == Vector3.zero)
            {
                gameObject.SetActive(false);
                helpPanelTween.PlayBackwards();
            }
            transform.localPosition = new Vector3(1920, 0, 0);
        }

        public override void EnterPanel()
        {
            base.EnterPanel();
            gameObject.SetActive(true);
            helpPageSlideScrollView.Init();
            towerPageSlideScrollView.Init();
            MoveToCenter();
        }

        public override void ExitPanel()
        {
            base.ExitPanel();

            //在冒险模式选择场景
            if (mUIFacade.currentWindowState.GetType() == typeof(NormalGameOptionWindowState))
            {
                mUIFacade.ChangeSceneState(new MainWindowState(mUIFacade));
                SceneManager.LoadScene(1);
            }
            else//如果是在主场景
            {
                helpPanelTween.PlayBackwards();
                mUIFacade.currentWindowPanelDict[StringManager.MainPanel].EnterPanel();
            }
        }

        public void MoveToCenter()
        {
            helpPanelTween.PlayForward();
        }
    }
}
