using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    /// <summary>
    /// 小关卡选择面板
    /// </summary>
    public class GameNormalLevelPanel : BasePanel
    {
        [SerializeField]
        private Transform levelContentTrans;

        [SerializeField]
        private SlideScrollView slideScrollView;

        [SerializeField]
        private Image img_BGLeft;

        [SerializeField]
        private Image img_BGRight;

        [SerializeField]
        private GameObject img_LockStartGame;//未解锁关卡遮挡板

        [SerializeField]
        private Transform TowerList;//建塔列表

        [SerializeField]
        private TMP_Text tex_TotalWaves;


        public int currentBigLevelID;
        public int currentLevelID;
        private string filePath;//图片资源加载的根路径
        private string theSpritePath;         
        private PlayerManager playerManager;
        private List<GameObject> levelContentImageGos;//实例化出来的地图卡片UI
        private List<GameObject> towerContentImageGos;//实例化出来的建塔列表UI

        protected override void Awake()
        {
            base.Awake();
            filePath = "GameOption/Normal/Level/";
            playerManager = mUIFacade.mPlayerManager;
            levelContentImageGos = new List<GameObject>();
            towerContentImageGos = new List<GameObject>();
            currentBigLevelID = 1;
            currentLevelID = 1;
        }

        //更新地图UI的方法，动态生成地图ui
        public void UpdateMapUI(string spritePath)
        {
            img_BGLeft.sprite = mUIFacade.GetSprite(spritePath + "BG_Left");
            img_BGRight.sprite = mUIFacade.GetSprite(spritePath + "BG_Right");
            for (int i = 0; i < 5; i++)
            {
                levelContentImageGos.Add(CreateUIAndSetUIPosition("LevelCard", levelContentTrans));
                //更换关卡图片
                levelContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(spritePath + "Level_" + (i + 1).ToString());
                Stage stage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + i];
                
                levelContentImageGos[i].transform.Find("Achievement ").Find("Carrot").gameObject.SetActive(false);
                levelContentImageGos[i].transform.Find("Achievement ").Find("AllClear").gameObject.SetActive(false);
                if (stage.unLocked)
                {
                    //已解锁
                    if (stage.mAllClear)
                    {
                        levelContentImageGos[i].transform.Find("Achievement ").Find("AllClear").gameObject.SetActive(true);
                    }
                    if (stage.mCarrotState != 0)
                    {
                        Image carrotImageGo = levelContentImageGos[i].transform.Find("Achievement ").Find("AllClear").GetComponent<Image>();
                        carrotImageGo.gameObject.SetActive(true);
                        carrotImageGo.sprite = mUIFacade.GetSprite(filePath + "Carrot_" + stage.mCarrotState);
                    }
                    levelContentImageGos[i].transform.Find("Lock").gameObject.SetActive(false);
                    levelContentImageGos[i].transform.Find("Mask").gameObject.SetActive(false);
                }
                else
                {
                    //未解锁
                    if (stage.mIsRewardLevel)//是奖励关卡
                    {
                        levelContentImageGos[i].transform.Find("Lock").gameObject.SetActive(false);
                        levelContentImageGos[i].transform.Find("Mask").gameObject.SetActive(true);
                        Image monsterPetImage = levelContentImageGos[i].transform.Find("Mask/MonsterImage").GetComponent<Image>();
                        monsterPetImage.sprite = mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + currentBigLevelID.ToString());
                        monsterPetImage.SetNativeSize();
                        monsterPetImage.transform.localScale = new Vector3(2, 2, 1);
                    }
                    else
                    {
                        //显示锁
                        levelContentImageGos[i].transform.Find("Lock").gameObject.SetActive(true);
                        //隐藏mask，
                        levelContentImageGos[i].transform.Find("Mask").gameObject.SetActive(false);
                    }
                }
            }
            //设置滚动视图Content的大小
            slideScrollView.Init();

        }

        //销毁地图卡
        private void DestoryMapUI()
        {
            if (levelContentImageGos.Count > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "LevelCard", levelContentImageGos[i]);
                }
                slideScrollView.InitScrollLength();
                levelContentImageGos.Clear();
            }
        }

        //更新静态UI
        public void UpdateLevelUI(string SpritePath)
        {
            if (towerContentImageGos.Count != 0)
            {
                for (int i = 0; i < towerContentImageGos.Count; i++)
                {
                    towerContentImageGos[i].GetComponent<Image>().sprite = null;
                    mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Tower", towerContentImageGos[i]);
                }
                towerContentImageGos.Clear();
            }

            Stage stage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
            if (stage.unLocked)
            {
                img_LockStartGame.SetActive(false);
            }
            else
            {
                img_LockStartGame.SetActive(true);
            }
            tex_TotalWaves.text = stage.mTotalRound.ToString();       
            for (int i = 0; i < stage.mTowerIDListLength; i++)
            {
                towerContentImageGos.Add(CreateUIAndSetUIPosition("Tower", TowerList));
                towerContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.
                    GetSprite(filePath + "Tower" + "/Tower_" + stage.mTowerIDList[i].ToString());
            }
        }


        /// <summary>
        /// 处理UI面板的方法
        /// </summary>
        /// <param name="currentBigLevel"></param>

        //外部调用的进入当前页面方法
        public void ToThisPanel(int currentBigLevel)
        {
            currentBigLevelID = currentBigLevel;
            currentLevelID = 1;
            EnterPanel();
        }

        public override void InitPanel()
        {
            base.InitPanel();
            gameObject.SetActive(false);
        }

        public override void EnterPanel()
        {
            base.EnterPanel();
            gameObject.SetActive(true);
            theSpritePath = filePath + currentBigLevelID.ToString() + "/";
            DestoryMapUI();
            UpdateMapUI(theSpritePath);
            UpdateLevelUI(theSpritePath);

        }

        public override void UpdatePanel()
        {
            base.UpdatePanel();
            UpdateLevelUI(theSpritePath);
        }



        public override void ExitPanel()
        {
            base.ExitPanel();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 帮助更新UI的一些方法
        /// </summary>
        private void LoadResource()
        {
            //成就系统图片
            mUIFacade.GetSprite(filePath + "AllClear");
            mUIFacade.GetSprite(filePath + "Carrot_1");
            mUIFacade.GetSprite(filePath + "Carrot_2");
            mUIFacade.GetSprite(filePath + "Carrot_3");

            for (int i = 1; i < 4; i++)
            {
                string spritePath = filePath + i.ToString() + "/";
                mUIFacade.GetSprite(spritePath + "BG_Left");
                mUIFacade.GetSprite(spritePath + "BG_Right");
                //地图图片
                for (int j = 1; j < 6; j++)
                {
                    mUIFacade.GetSprite(spritePath + "Level_" + j.ToString());
                }
                for (int j = 1; j < 13; j++)
                {
                    mUIFacade.GetSprite(filePath + "Tower/Tower_" + j.ToString());
                }
            }
        }

        //实例化UI
        public GameObject CreateUIAndSetUIPosition(string uiName, Transform parentTrans)
        {
            GameObject itemGo = mUIFacade.GetGameObjectResource(FactoryType.UIFactory, uiName);
            itemGo.transform.SetParent(parentTrans);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
            return itemGo;
        }

        public void ToNextLevel()
        {
            currentLevelID++;
            UpdatePanel();
        }

        public void ToLastLevel()
        {
            currentLevelID--;
            UpdatePanel();
        }

        public void ToGamePanel()
        {
            GameManager.Instance.currentStage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
            mUIFacade.currentWindowPanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new NormalModelWindowState(mUIFacade));
        }
    }
}
