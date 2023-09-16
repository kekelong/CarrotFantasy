using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Carrot
{
    public class GridPoint : MonoBehaviour
    {
        #region Struct
        /// <summary>
        /// Grid State
        /// </summary>
        [System.Serializable]
        public struct GridState
        {
            public bool canBuild;
            public bool isMonsterPoint;
            public bool hasItem;
            public int itemID;
        }

        /// <summary>
        /// Grid Index
        /// </summary>
        [System.Serializable]
        public struct GridIndex
        {
            public int xIndex;
            public int yIndex;
        }
        #endregion


        #region Public Attribute
        public GridState gridState;
        public GridIndex gridIndex;
        public bool hasTower;
        #endregion


        #region Private Attribute

        /// <summary>
        /// SpriteRenderer Rreferenced
        /// </summary>
        private SpriteRenderer spriteRenderer;


        /// <summary>
        /// 格子图片资源
        /// </summary>
        private Sprite gridSprite;

        /// <summary>
        /// 开始时格子的图片显示
        /// </summary>
        private Sprite startSprite;

        /// <summary>
        /// 禁止建塔
        /// </summary>
        private Sprite cantBuildSprite;

        /// <summary>
        /// 建塔列表
        /// </summary>
        private GameObject towerListGO;

        /// <summary>
        /// 操作塔的按钮画布
        /// </summary>
        public GameObject handleTowerGanvasGo;


        /// <summary>
        /// 炮塔GO
        /// </summary>
        public GameObject towerGo;

        /// <summary>
        /// Tower Script Rreferenced
        /// </summary>
        public Tower tower;
        public TowerPersonalProperty towerPersonalProperty;

        /// <summary>
        /// 炮塔等级
        /// </summary>
        public int towerLevel;

        /// <summary>
        /// 是否可升级信号
        /// </summary>
        private GameObject levelUpSignalGo;
        #endregion


        #region Temporary Variable

        private Transform upLevelButtonTrans;//两个按钮的trans引用
        private Transform sellTowerButtonTrans;
        private Vector3 upLevelButtonInitPos;//两个按钮的初始位置
        private Vector3 sellTowerButtonInitPos;

        #endregion


        #region MonoBehaviour Callback
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            InitGrid();
            gridSprite = GameController.Instance.GetSprite("NormalMordel/Game/Grid");
            startSprite = GameController.Instance.GetSprite("NormalMordel/Game/StartSprite");
            cantBuildSprite = GameController.Instance.GetSprite("NormalMordel/Game/cantBuild");
            spriteRenderer.sprite = startSprite;
            Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color=toColor, new Color(1, 1, 1, 0.2f), 3);
            t.OnComplete(ChangeSpriteToGrid);
            towerListGO = GameController.Instance.TowerListCanvas;
            handleTowerGanvasGo = GameController.Instance.handleTowerCanvasGo;
            upLevelButtonTrans = handleTowerGanvasGo.transform.Find("UpLevel");
            sellTowerButtonTrans = handleTowerGanvasGo.transform.Find("Sell");
            upLevelButtonInitPos = upLevelButtonTrans.localPosition;
            sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;

            levelUpSignalGo = transform.Find("LevelUp").gameObject;
            levelUpSignalGo.SetActive(false);

        }

        private void Update()
        {
            if (levelUpSignalGo != null)
            {
                if (hasTower)
                {
                    if (towerPersonalProperty.upLevelPrice <= GameController.Instance.coin && towerLevel < 3)
                    {
                        levelUpSignalGo.SetActive(true);
                    }
                    else
                    {
                        levelUpSignalGo.SetActive(false);
                    }
                }
                else
                {
                    if (levelUpSignalGo.activeSelf)
                    {
                        levelUpSignalGo.SetActive(false);
                    }
                }
            }
        }


        /// <summary>
        /// 有关格子处理的方法
        /// </summary>
        private void OnMouseDown()
        {
            //选择的是UI则不发生交互
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            GameController.Instance.HandleGrid(this);
        }

        #endregion


        /// <summary>
        /// 改回原来样式的Sprite
        /// </summary>
        private void ChangeSpriteToGrid()
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);

            if (gridState.canBuild)
            {
                spriteRenderer.sprite = gridSprite;
            }
            else
            {
                spriteRenderer.sprite = cantBuildSprite;
            }
        }

        /// <summary>
        /// 初始化格子
        /// </summary>
        public void InitGrid()
        {
            gridState.canBuild = true;
            gridState.isMonsterPoint = false;
            gridState.hasItem = false;
            spriteRenderer.enabled = true;
            gridState.itemID = -1;
            towerGo = null;
            towerPersonalProperty = null;
            hasTower = false;

        }


        /// <summary>
        /// 更新格子状态
        /// </summary>
        public void UpdateGrid()
        {
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
                if (gridState.hasItem)
                {
                    CreateItem();
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }


        /// <summary>
        /// 创建物品
        /// </summary>
        private void CreateItem()
        {
            GameObject itemGo = GameController.Instance.GetGameObjectResource(GameController.Instance.mapController.bigLevelID.ToString() + "/Item/" + gridState.itemID);
            itemGo.GetComponent<Item>().itemID = gridState.itemID;
            itemGo.transform.SetParent(GameController.Instance.transform);

            Vector3 createPos = transform.position - new Vector3(0, 0, 3);
            if (gridState.itemID <= 2)
            {
                createPos += new Vector3(GameController.Instance.mapController.gridWidth, -GameController.Instance.mapController.gridHeight) / 2;
            }
            else if (gridState.itemID <= 4)
            {
                createPos += new Vector3(GameController.Instance.mapController.gridWidth, 0) / 2;
            }
            itemGo.transform.position = createPos;
            itemGo.GetComponent<Item>().gridPoint = this;
        }



        public void ShowGrid()
        {
            if (!hasTower)
            {
                spriteRenderer.enabled = true;
                //显示建塔列表
                towerListGO.transform.position = CorrectTowerListGoPosition();
                towerListGO.SetActive(true);
            }
            else
            {
                handleTowerGanvasGo.transform.position = transform.position;
                CorrectHandleTowerCanvasGoPosition();
                handleTowerGanvasGo.SetActive(true);
                //显示塔的攻击范围
                towerGo.transform.Find("attackRange").gameObject.SetActive(true);
            }
        }


        public void HideGrid()
        {
            if (!hasTower)
            {
                //隐藏建塔列表
                towerListGO.SetActive(false);
            }
            else
            {
                handleTowerGanvasGo.SetActive(false);
                //隐藏塔的范围
                towerGo.transform.Find("attackRange").gameObject.SetActive(false);
            }
            spriteRenderer.enabled = false;
        }

        /// <summary>
        /// 显示此格子不能够去建塔
        /// </summary>
        public void ShowCantBuild()
        {
            spriteRenderer.enabled = true;
            Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 2f);
            t.OnComplete(() =>
            {
                spriteRenderer.enabled = false;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            });
        }

        /// <summary>
        /// 纠正建塔列表的位置
        /// </summary>
        private Vector3 CorrectTowerListGoPosition()
        {
            Vector3 correctPosition = Vector3.zero;
            if (gridIndex.xIndex <= 3 && gridIndex.xIndex >= 0)
            {
                correctPosition += new Vector3(GameController.Instance.mapController.gridWidth, 0, 0);
            }
            else if (gridIndex.xIndex <= 11 && gridIndex.xIndex >= 8)
            {
                correctPosition -= new Vector3(GameController.Instance.mapController.gridWidth, 0, 0);
            }

            if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)
            {
                correctPosition += new Vector3(0, GameController.Instance.mapController.gridHeight, 0);
            }
            else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)
            {
                correctPosition -= new Vector3(0, GameController.Instance.mapController.gridHeight, 0);
            }
            correctPosition += transform.position;
            return correctPosition;
        }

        /// <summary>
        /// 纠正操作塔UI画布的方法(纠正按钮位置的方法)
        /// </summary>
        private void CorrectHandleTowerCanvasGoPosition()
        {
            upLevelButtonTrans.localPosition = Vector3.zero;
            sellTowerButtonTrans.localPosition = Vector3.zero;
            if (gridIndex.yIndex <= 0)
            {
                if (gridIndex.xIndex == 0)
                {
                    sellTowerButtonTrans.position += new Vector3(GameController.Instance.mapController.gridWidth * 3 / 4, 0, 0);
                }
                else
                {
                    sellTowerButtonTrans.position -= new Vector3(GameController.Instance.mapController.gridWidth * 3 / 4, 0, 0);
                }
                upLevelButtonTrans.localPosition = upLevelButtonInitPos;
            }
            else if (gridIndex.yIndex >= 6)
            {
                if (gridIndex.xIndex == 0)
                {
                    upLevelButtonTrans.position += new Vector3(GameController.Instance.mapController.gridWidth * 3 / 4, 0, 0);
                }
                else
                {
                    upLevelButtonTrans.position -= new Vector3(GameController.Instance.mapController.gridWidth * 3 / 4, 0, 0);
                }
                sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
            }
            else
            {
                upLevelButtonTrans.localPosition = upLevelButtonInitPos;
                sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
            }
        }

        /// <summary>
        /// 建塔后的处理方法
        /// </summary>
        public void AfterBuild()
        {
            spriteRenderer.enabled = false;
            towerGo = transform.GetChild(1).gameObject;
            tower = towerGo.GetComponent<Tower>();
            towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
            towerLevel = towerPersonalProperty.towerLevel;
        }

    }
}

