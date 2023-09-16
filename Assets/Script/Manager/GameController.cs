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

        //����
        public Level level;
        private GameManager mGameManager;
        public int[] mMonsterIDList;//��ǰ���εĲ����б�
        public Stage currentStage;
        public MapController mapController;
        public Transform targetTrans;//����Ŀ��
        public GameObject targetSignal;//�����ź�
        public GridPoint selectGrid;//��ǰѡ��ĸ���

        //��Ϸ��Դ
        public RuntimeAnimatorController[] controllers; //����Ķ������ſ�����

        //��ϷUI�����
        public NormalModelpanel normalModelpanel;

        //���ڼ����ĳ�Ա����
        public int killMonsterNum; //��ǰ����ɱ����
        public int clearItemNum;//������������
        public int killMonsterTotalNum;//ɱ������
        public int mMonsterIDIndex;  //����ͳ�Ƶ�ǰ�����б�������������

        //����ֵ
        public int carrotHp = 10;
        public int coin;
        public int gameSpeed; //��ǰ��Ϸ�����ٶ�
        public bool isPause;
        public bool creatingMonster;//�Ƿ��������
        public bool gameOver;  //��Ϸ�Ƿ����

        //������
        public MonsterBuilder monsterBuilder;
        public TowerBuilder towerBuilder;

        //�����йصĳ�Ա����
        public Dictionary<int, int> towerPriceDict;//�����۸��  
        public GameObject TowerListCanvas; //������ť�б�
        public GameObject handleTowerCanvasGo; //�����������������Ļ���



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
            //��Ա������ֵ
            gameSpeed = 1;
            coin = 1000;

            monsterBuilder = new MonsterBuilder();
            towerBuilder = new TowerBuilder();

            //�����б�Ĵ���
            for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
            {
                GameObject itemGo = mGameManager.GetGameObjectResource(FactoryType.UIFactory, "BuildTowerButton");
                itemGo.transform.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
                itemGo.transform.SetParent(TowerListCanvas.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one;
            }
            //�����۸��
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
                //�����߼�
                //���غϽ�������ʼ��һ�غ�
                if (killMonsterNum>=mMonsterIDList.Length)
                {
                    //��ӵ�ǰ�غ���������
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
                //��ͣ
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
        /// ���Ӵ�����
        /// </summary>
        public void HandleGrid(GridPoint grid)//��ǰѡ��ĸ���
        {
            if (grid.gridState.canBuild)
            {
                if (selectGrid == null)//û����һ������
                {
                    selectGrid = grid;
                    selectGrid.ShowGrid();

                }
                else if (grid == selectGrid)//ѡ��ͬһ������
                {
                    grid.HideGrid();
                    selectGrid = null;
                }
                else if (grid != selectGrid)//ѡ�в�ͬ����
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
        /// ������ҽ��
        /// </summary>
        public void ChangeCoin(int coinNum)
        {
            coin += coinNum;
            //������ϷUI��ʾ

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

        //�ܲ�Ѫ������
        public void DecreaseHP()
        {
            carrotHp--;
            //�����ܲ�UI��ʾ
            mapController.carrot.UpdateCarrotUI();
        }

        /// <summary>
        /// �뼯��Ŀ���йصķ���
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
        /// ��������
        /// </summary>
        public void CreateMonster()
        {
            creatingMonster = true;
            InvokeRepeating("InstantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
        }
        /// <summary>
        /// ������ַ���
        /// </summary>
        private void InstantiateMonster()
        {            if (mMonsterIDIndex >= mMonsterIDList.Length)
            {
                StopCreateMonster();
            }
            //������Ч
            GameObject effectGo = GetGameObjectResource("SpawnEffect");
            effectGo.transform.SetParent(transform);
            effectGo.transform.position = mapController.monsterPathPos[0];


            //��������
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
        /// ֹͣ����
        /// </summary>
        public void StopCreateMonster()
        {
            CancelInvoke();
        }


        /// <summary>
        /// ���ӵ�ǰ�غ��������ҽ�����һ���غ����������
        /// </summary>
        public void AddRoundNum()
        {
            mMonsterIDIndex = 0;
            killMonsterNum = 0;
            level.AddRoundNum();
            level.HandleRound();
            //�����йػغ���ʾ��UI
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

