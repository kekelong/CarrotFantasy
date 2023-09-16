using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Carrot
{
    /// <summary>
    /// 
    /// </summary>
    public class NormalModelpanel : BasePanel
    {
        public GameObject topMenu;
        public GameObject mainMenuPage;
        public GameObject gameOverPage;
        public GameObject gameWinPage;
        public GameObject startUI;
        public GameObject FinalWave;

        public TopMenuPage TopMenuPage;


        public int TotalRound;
        protected override void Awake()
        {
            startUI = transform.Find("StartUI").gameObject;
            base.Awake();
            transform.SetSiblingIndex(1);
            TopMenuPage = topMenu.GetComponent<TopMenuPage>();
        }

        private void OnEnable()
        {
            startUI.SetActive(true);
            
            InvokeRepeating("PlayAudio", 0, 1);
            Invoke("StartGame", 3);

        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void PlayAudio()
        {
            
        }

        private void StartGame()
        {
            GameController.Instance.StartGame();
            startUI.SetActive(false);
            UpdatePlaymanagerData();
            CancelInvoke();
        }

        public override void EnterPanel()
        {
            base.EnterPanel();
            TotalRound = GameController.Instance.currentStage.mTotalRound;
            TopMenuPage.Text_TotalCount.text = " " + "/" + TotalRound+"Monster";
            topMenu.SetActive(true);
        }

        public override void UpdatePanel()
        {
            base.UpdatePanel();
            TopMenuPage.UpdateCoinText(); 
            TopMenuPage.UpdatRoundText(); 

        }


        public void ShowRoundText(TMP_Text text)
        {
            int roundNum = GameController.Instance.level.currentRound + 1;
            string roundstr = "";
            if (roundNum < 10)
            {
                roundstr = "0 " + roundNum.ToString();
            }
            else
            {
                roundstr = (roundNum/10).ToString() + " " + (roundNum% 10).ToString();
            }

            text.text = roundstr;
        }


        public void ShowMainMenuPage()
        {
            GameController.Instance.isPause = true;
            mainMenuPage.SetActive(true);
        }

        public void CloseMainMenuPage()
        {
            GameController.Instance.isPause = false;
            mainMenuPage.SetActive(false);
        }

        public void ShowGameWinPage()
        {
            Stage stage = GameManager.Instance.PlayerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.mLevelID-1 + (GameController.Instance.currentStage.mBigLevelID-1)*5];
            if (GameController.Instance.LevelAllClear())
            {
                stage.mAllClear = true;
            }

            int carrotState = GameController.Instance.getCarrotState();
            if (carrotState != 0 )
            {
                if(stage.mCarrotState > carrotState)
                {
                    stage.mCarrotState = carrotState;
                }
            }

            if(GameController.Instance.currentStage.mLevelID % 5 !=0 && 
                (GameController.Instance.currentStage.mLevelID - 1 + (GameController.Instance.currentStage.mBigLevelID - 1) * 5) < GameManager.Instance.PlayerManager.unLockedNormalModelLevelList.Count)
            {
                GameManager.Instance.PlayerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5].unLocked = true;
            }   

            UpdatePlaymanagerData();
            gameWinPage.SetActive(true);
            GameController.Instance.gameOver = false; 

            GameManager.Instance.PlayerManager.adventrueModelNum++;
        }


        public void ShowGameOverPage()
        {
            UpdatePlaymanagerData();
            gameOverPage.SetActive(true);
            GameController.Instance.gameOver = false;
        }

        public void ShowFinalWave()
        {
            FinalWave.SetActive(true);
            Invoke("CloseFianlWave", 1.05f);
        }

        public void CloseFianlWave()
        {
            FinalWave.SetActive(false);
        }

        private void UpdatePlaymanagerData()
        {
            GameManager.Instance.PlayerManager.coin += GameController.Instance.coin;
            GameManager.Instance.PlayerManager.killMonsterNum += GameController.Instance.killMonsterNum;
            GameManager.Instance.PlayerManager.clearItemNum += GameController.Instance.clearItemNum;
        }
        public void RePlayGame()
        {
            UpdatePlaymanagerData();
            mUIFacade.ChangeSceneState(new NormalModelWindowState(mUIFacade));
            GameController.Instance.gameOver = false;
            Invoke("ResetGame", 2);
            
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(4);
            ResetUI();
            gameObject.SetActive(true);
        }

        public void ResetUI()
        {
            gameOverPage.SetActive(false);
            gameWinPage.SetActive(false );
            mainMenuPage.SetActive(false);
            gameObject.SetActive(false);
        }

        public void SelectOtherLevel()
        {
            GameController.Instance.gameOver = false;
            UpdatePlaymanagerData();
            Invoke("ToOtherScene",2);
            mUIFacade.ChangeSceneState(new NormalGameOptionWindowState(mUIFacade));
        }

        public void ToOtherScene()
        {
            GameController.Instance.gameOver = false;
            ResetUI();
            SceneManager.LoadScene(2);
        }

    }
}

