using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Carrot
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        private static GameController _Instance;
        public static GameController Instance { get { return _Instance; } }
        #endregion

        //引用
        public Level level;
        private GameManager mGameManager;
        public int[] mMonsterIDList;//当前波次的产怪列表
        public Stage currentStage;
        public MapController mapController;
        public Transform targetTrans;//集火目标
        public GameObject targetSignal;//集火信号
        public GridPoint selectGrid;//当前选择的格子

        //游戏资源
        public RuntimeAnimatorController[] controllers; //怪物的动画播放控制器

        //游戏UI的面板
        public NormalModelpanel normalModelpanel;

        //用于计数的成员变量
        public int killMonsterNum; //当前波次杀怪数
        public int clearItemNum;//道具销毁数量
        public int killMonsterTotalNum;//杀怪总数
        public int mMonsterIDIndex;  //用于统计当前怪物列表产生怪物的索引

        //属性值
        public int carrotHp = 10;
        public int coin;
        public int gameSpeed; //当前游戏进行速度
        public bool isPause;
        public bool creatingMonster;//是否继续产怪
        public bool gameOver;  //游戏是否结束

        //建造者
        public MonsterBuilder monsterBuilder;
        public TowerBuilder towerBuilder;

        //建塔有关的成员变量
        public Dictionary<int, int> towerPriceDict;//建塔价格表  
        public GameObject TowerListCanvas; //建塔按钮列表
        public GameObject handleTowerCanvasGo; //处理塔升级与买卖的画布



        private void Awake()
        {
            _Instance = this;
            mGameManager = GameManager.Instance;
            currentStage = mGameManager.currentStage;
            normalModelpanel = mGameManager.UIWindowManager.mUIFacade.currentWindowPanelDict[StringManager.NormalModelPanel] as NormalModelpanel;
            normalModelpanel.EnterPanel();
            mapController = GetComponent<MapController>();
            mapController.InitMapMaker();
            mapController.LoadMap(currentStage.mBigLevelID, currentStage.mLevelID);
            //成员变量赋值
            gameSpeed = 1;
            coin = 1000;

            monsterBuilder = new MonsterBuilder();
            towerBuilder = new TowerBuilder();

            //建塔列表的处理
            for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
            {
                GameObject itemGo = mGameManager.GetGameObjectResource(FactoryType.UIFactory, "BuildTowerButton");
                itemGo.transform.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
                itemGo.transform.SetParent(TowerListCanvas.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one;
            }
            //建塔价格表
            towerPriceDict = new Dictionary<int, int>
            {
                { 1,100},
                { 2,120},
                { 3,140},
                { 4,160},
                { 5,160}
            };

            controllers = new RuntimeAnimatorController[12];
            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i] = GetRuntimeAnimatorController("Monster/" + mapController.bigLevelID.ToString() + "/" + (i + 1).ToString());
            }
            level = new Level(mapController.roundInfoList.Count, mapController.roundInfoList);
            normalModelpanel.TopMenuPage.UpdateCoinText();
            normalModelpanel.TopMenuPage.UpdatRoundText();
            isPause = true;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isPause)
            {
                //产怪逻辑
                //本回合结束，开始下一回合
                if (killMonsterNum>=mMonsterIDList.Length)
                {
                    //添加当前回合数的索引
                    AddRoundNum();
                }
                else
                {
                    if (!creatingMonster)
                    {
                        CreateMonster();
                    }
                }
            }
            else
            {
                //暂停
                StopCreateMonster();
                creatingMonster = false;
            }

        }

        public void StartGame()
        {
            isPause = false;
            level.HandleRound();
        }

        /// <summary>
        /// 格子处理方法
        /// </summary>
        public void HandleGrid(GridPoint grid)//当前选择的格子
        {
            if (grid.gridState.canBuild)
            {
                if (selectGrid == null)//没有上一个格子
                {
                    selectGrid = grid;
                    selectGrid.ShowGrid();

                }
                else if (grid == selectGrid)//选中同一个格子
                {
                    grid.HideGrid();
                    selectGrid = null;
                }
                else if (grid != selectGrid)//选中不同格子
                {
                    selectGrid.HideGrid();
                    selectGrid = grid;
                    selectGrid.ShowGrid();
                }
            }
            else
            {
                grid.HideGrid();
                grid.ShowCantBuild();
                if (selectGrid != null)
                {
                    selectGrid.HideGrid();
                }
            }
        }
        /// <summary>
        /// 更新玩家金币
        /// </summary>
        public void ChangeCoin(int coinNum)
        {
            coin += coinNum;
            //更新游戏UI显示

            normalModelpanel.UpdatePanel();
        }

        public bool LevelAllClear()
        {
            for (int x = 0; x < MapController.xColumn; x++)
            {
                for (int y = 0; y < MapController.yRow; y++)
                {
                    if (mapController.gridPoints[x,y].gridState.hasItem)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int getCarrotState()
        {
            int carrotstate = 0;
            if(carrotHp >= 4)
            {
                carrotstate = 1;
            }
            else if(carrotHp >= 2)
            {
                carrotstate = 2;
            }
            else
            {
                carrotstate = 3;
            }

            return carrotstate;

        }

        //萝卜血量减少
        public void DecreaseHP()
        {
            carrotHp--;
            //更新萝卜UI显示
            mapController.carrot.UpdateCarrotUI();
        }

        /// <summary>
        /// 与集火目标有关的方法
        /// </summary>
        public void ShowSignal()
        {
            targetSignal.transform.position = targetTrans.position + new Vector3(0, mapController.gridHeight / 2, 0);
            targetSignal.transform.SetParent(targetTrans);
            targetSignal.SetActive(true);
        }

        public void HideSignal()
        {
            targetSignal.transform.SetParent(transform);
            targetSignal.gameObject.SetActive(false);
            targetTrans = null;
        }

        #region Spawn Monster
        /// <summary>
        /// 产生怪物
        /// </summary>
        public void CreateMonster()
        {
            creatingMonster = true;
            InvokeRepeating("InstantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
        }
        /// <summary>
        /// 具体产怪方法
        /// </summary>
        private void InstantiateMonster()
        {            if (mMonsterIDIndex >= mMonsterIDList.Length)
            {
                StopCreateMonster();
            }
            //产生特效
            GameObject effectGo = GetGameObjectResource("SpawnEffect");
            effectGo.transform.SetParent(transform);
            effectGo.transform.position = mapController.monsterPathPos[0];


            //产生怪物
            if (mMonsterIDIndex < mMonsterIDList.Length)
            {
                monsterBuilder.m_monsterID = level.roundList[level.currentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
            }

            GameObject monsterGo = monsterBuilder.GetProduct();
            monsterGo.transform.SetParent(transform);
            monsterGo.transform.position = mapController.monsterPathPos[0];
            mMonsterIDIndex++;

        }
        /// <summary>
        /// 停止产生
        /// </summary>
        public void StopCreateMonster()
        {
            CancelInvoke();
        }


        /// <summary>
        /// 增加当前回合数，并且交给下一个回合来处理产怪
        /// </summary>
        public void AddRoundNum()
        {
            mMonsterIDIndex = 0;
            killMonsterNum = 0;
            level.AddRoundNum();
            level.HandleRound();
            //更新有关回合显示的UI
            normalModelpanel.UpdatePanel();
        }

        #endregion

        #region Get Resources
        public Sprite GetSprite(string resourcePath)
        {
            return mGameManager.GetSprite(resourcePath);
        }
        public AudioClip GetAudioClip(string resourcePath)
        {
            return mGameManager.GetAudioClip(resourcePath);
        }
        public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
        {
            return mGameManager.GetRunTimeAnimatorController(resourcePath);
        }
        public GameObject GetGameObjectResource(string resourcePath)
        {
            return mGameManager.GetGameObjectResource(FactoryType.GameFactory, resourcePath);
        }
        public void PushGameObjectToFactory(string resourcePath, GameObject itemGo)
        {
            mGameManager.PushGameObjectToFactory(FactoryType.GameFactory, resourcePath, itemGo);
        }

        #endregion
    }
}

