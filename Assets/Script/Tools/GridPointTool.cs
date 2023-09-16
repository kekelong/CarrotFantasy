using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// 格子工具脚本：存贮塔，塔子信息，
    /// </summary>
    public class GridPointTool : MonoBehaviour
    {
        //属性
        private SpriteRenderer spriteRenderer;
        public GridState gridState;
        public GridIndex gridIndex;
        public bool hasTower;

        //资源
        private Sprite gridSprite;//格子图片资源
        private Sprite startSprite;//开始时格子的图片显示
        private Sprite cantBuildSprite;//禁止建塔


        private Sprite monsterPathSprite;//怪物路点图片资源
        public GameObject[] itemPrefabs;//道具数组
        public GameObject currentItem; //当前格子持有道具

        //格子状态
        [System.Serializable]
        public struct GridState
        {
            public bool canBuild;
            public bool isMonsterPoint;
            public bool hasItem;
            public int itemID;
        }

        //格子索引
        [System.Serializable]
        public struct GridIndex
        {
            public int xIndex;
            public int yIndex;
        }

        //引用
        private GameController gameController;
        private GameObject towerListGo;//当前关卡建塔列表
        public GameObject handleTowerGanvasGo;//有塔之后的操作按钮画布
        private Transform upLevelButtonTrans;//两个按钮的trans引用
        private Transform sellTowerButtonTrans;
        private Vector3 upLevelButtonInitPos;//两个按钮的初始位置
        private Vector3 sellTowerButtonInitPos;


        private void Awake()
        {

            gridSprite= Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
            monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
            //宝箱的预制体
            itemPrefabs = new GameObject[10];
            string prefabsPath = "Prefabs/Game/" + MapMakerTool.Instance.bigLevelID.ToString()+"/Item/";
            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
                if (!itemPrefabs[i])
                {
                    Debug.Log("加载失败，失败路径："+prefabsPath+i); 
                }
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
            InitGrid();
        }

        private void Update()
        { 
        }

        //改回原来样式的Sprite
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

        //初始化格子
        public void InitGrid()
        {
            gridState.canBuild = true;
            gridState.isMonsterPoint = false;
            gridState.hasItem = false;
            spriteRenderer.enabled = true;
            gridState.itemID = -1;
            spriteRenderer.sprite = gridSprite;
            Destroy(currentItem); 
        }

        private void OnMouseDown()
        {
            //怪物路点
            if (Input.GetKey(KeyCode.P))
            {
                gridState.canBuild = false;
                spriteRenderer.enabled = true;
                gridState.isMonsterPoint = !gridState.isMonsterPoint;
                if (gridState.isMonsterPoint)//是怪物路点
                {
                    MapMakerTool.Instance.monsterPath.Add(gridIndex);
                    spriteRenderer.sprite = monsterPathSprite;
                }
                else//不是怪物路点
                {
                    MapMakerTool.Instance.monsterPath.Remove(gridIndex);
                    spriteRenderer.sprite = gridSprite;
                    gridState.canBuild = true;
                }
            }
            //道具
            else if (Input.GetKey(KeyCode.I))
            {
                gridState.itemID++;
                //当前格子从持有道具状态转化为没有道具
                if (gridState.itemID == itemPrefabs.Length)
                {
                    gridState.itemID = -1;
                    Destroy(currentItem);
                    gridState.hasItem = false;
                    return;
                }
                if (currentItem == null)
                {
                    //产生道具
                    CreateItem();
                }
                else//本身就有道具
                {
                    Destroy(currentItem);
                    //产生道具
                    CreateItem();
                }
                gridState.hasItem = true;
            }
            else if (!gridState.isMonsterPoint)
            {
                gridState.isMonsterPoint = false;
                gridState.canBuild = !gridState.canBuild;
                if (gridState.canBuild)
                {
                    spriteRenderer.enabled = true;
                }
                else
                {
                    spriteRenderer.enabled = false;
                }
            }
        }

        //生成道具
        private void CreateItem()
        {
            Vector3 creatPos = transform.position;
            if (gridState.itemID<=2)
            {
                creatPos += new Vector3(MapMakerTool.Instance.gridWidth,-MapMakerTool.Instance.gridHeight)/2;
            }
            else if (gridState.itemID<=4)
            {
                creatPos += new Vector3(MapMakerTool.Instance.gridWidth,0)/2;
            }
            GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID],creatPos,Quaternion.identity);
            currentItem = itemGo;
        }

        //更新格子状态
        public void UpdateGrid()
        {
            if (gridState.canBuild)
            {
                spriteRenderer.sprite = gridSprite;
                spriteRenderer.enabled = true;
                if (gridState.hasItem)
                {
                    CreateItem();
                }
            }
            else
            {
                if (gridState.isMonsterPoint)
                {
                    spriteRenderer.sprite = monsterPathSprite;
                }
                else
                {
                    spriteRenderer.enabled = false;
                }
       
            }
        }



    }
}

