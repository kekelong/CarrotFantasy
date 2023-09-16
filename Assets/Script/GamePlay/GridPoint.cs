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
        /// ����ͼƬ��Դ
        /// </summary>
        private Sprite gridSprite;

        /// <summary>
        /// ��ʼʱ���ӵ�ͼƬ��ʾ
        /// </summary>
        private Sprite startSprite;

        /// <summary>
        /// ��ֹ����
        /// </summary>
        private Sprite cantBuildSprite;

        /// <summary>
        /// �����б�
        /// </summary>
        private GameObject towerListGO;

        /// <summary>
        /// �������İ�ť����
        /// </summary>
        public GameObject handleTowerGanvasGo;


        /// <summary>
        /// ����GO
        /// </summary>
        public GameObject towerGo;

        /// <summary>
        /// Tower Script Rreferenced
        /// </summary>
        public Tower tower;
        public TowerPersonalProperty towerPersonalProperty;

        /// <summary>
        /// �����ȼ�
        /// </summary>
        public int towerLevel;

        /// <summary>
        /// �Ƿ�������ź�
        /// </summary>
        private GameObject levelUpSignalGo;
        #endregion


        #region Temporary Variable

        private Transform upLevelButtonTrans;//������ť��trans����
        private Transform sellTowerButtonTrans;
        private Vector3 upLevelButtonInitPos;//������ť�ĳ�ʼλ��
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
        /// �йظ��Ӵ���ķ���
        /// </summary>
        private void OnMouseDown()
        {
            //ѡ�����UI�򲻷�������
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            GameController.Instance.HandleGrid(this);
        }

        #endregion


        /// <summary>
        /// �Ļ�ԭ����ʽ��Sprite
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
        /// ��ʼ������
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
        /// ���¸���״̬
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
        /// ������Ʒ
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
                //��ʾ�����б�
                towerListGO.transform.position = CorrectTowerListGoPosition();
                towerListGO.SetActive(true);
            }
            else
            {
                handleTowerGanvasGo.transform.position = transform.position;
                CorrectHandleTowerCanvasGoPosition();
                handleTowerGanvasGo.SetActive(true);
                //��ʾ���Ĺ�����Χ
                towerGo.transform.Find("attackRange").gameObject.SetActive(true);
            }
        }


        public void HideGrid()
        {
            if (!hasTower)
            {
                //���ؽ����б�
                towerListGO.SetActive(false);
            }
            else
            {
                handleTowerGanvasGo.SetActive(false);
                //�������ķ�Χ
                towerGo.transform.Find("attackRange").gameObject.SetActive(false);
            }
            spriteRenderer.enabled = false;
        }

        /// <summary>
        /// ��ʾ�˸��Ӳ��ܹ�ȥ����
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
        /// ���������б��λ��
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
        /// ����������UI�����ķ���(������ťλ�õķ���)
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
        /// ������Ĵ�����
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

