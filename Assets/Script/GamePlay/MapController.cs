using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

namespace Carrot
{
    public class MapController : MonoBehaviour
    {
        public GameObject gridGo;

        #region ��ͼ���й�����
        //��ͼ
        private float mapWidth;
        private float mapHeight;
        //����
        [HideInInspector]
        public float gridWidth;
        [HideInInspector]
        public float gridHeight;
        //��ǰ�ؿ�����
        public int bigLevelID;
        public int levelID;
        //ȫ���ĸ��Ӷ���
        public GridPoint[,] gridPoints;

        //����
        public const int yRow = 8;
        public const int xColumn = 12;

        #endregion


        //����·����
        [HideInInspector]
        public List<GridPoint.GridIndex> monsterPath;

        //����·����ľ���λ��
        [HideInInspector]
        public List<Vector3> monsterPathPos;

        private SpriteRenderer bgSR;
        private SpriteRenderer roadSR;

        //ÿһ���β����Ĺ���ID�б�
        public List<Round.RoundInfo> roundInfoList;

        [HideInInspector]
        public Carrot carrot;

        private void Awake()
        {
        }


        /// <summary>
        /// ��ʼ����ͼ
        /// </summary>
        public void InitMapMaker()
        {
            CalculateSize();
            gridPoints = new GridPoint[xColumn, yRow];
            monsterPath = new List<GridPoint.GridIndex>();
            for (int x = 0; x < xColumn; x++)
            {
                for (int y = 0; y < yRow; y++)
                {
                    GameObject itemGo = GameController.Instance.GetGameObjectResource("Grid");
                    itemGo.transform.position = CorretPostion(x * gridWidth, y * gridHeight);
                    itemGo.transform.SetParent(transform);
                    gridPoints[x, y] = itemGo.GetComponent<GridPoint>();
                    gridPoints[x, y].gridIndex.xIndex = x;
                    gridPoints[x, y].gridIndex.yIndex = y;
                }
            }
            bgSR = transform.Find("GmaeBackGround").GetComponent<SpriteRenderer>();
            roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();

        }

        /// <summary>
        /// ���ص�ͼ
        /// </summary>
        public void LoadMap(int bigLevel, int level)
        {
            bigLevelID = bigLevel;
            levelID = level;
            LoadLevelInfo(LoadLevelInfoFile("Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json"));
            monsterPathPos = new List<Vector3>();
            for (int i = 0; i < monsterPath.Count; i++)
            {
                monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
            }

            //��ʼ������ֹ��
            GameObject startPointGo = GameController.Instance.GetGameObjectResource("StartPoint");
            startPointGo.transform.position = monsterPathPos[0];
            startPointGo.transform.SetParent(transform);

            GameObject endPointGo = GameController.Instance.GetGameObjectResource("Carrot");
            endPointGo.transform.position = monsterPathPos[monsterPathPos.Count - 1] - new Vector3(0, 0, 1);
            endPointGo.transform.SetParent(transform);
            carrot = endPointGo.GetComponent<Carrot>();
        }


        /// <summary>
        /// ����Gridλ��,�������
        /// </summary>
        public Vector3 CorretPostion(float x, float y)
        {
            return new Vector3(x - mapWidth / 2 + gridWidth / 2, y - mapHeight / 2 + gridHeight / 2);
        }
        //�����ͼ���ӿ��
        private void CalculateSize()
        {
            //����
            Vector3 leftDown = new Vector3(0, 0);
            Vector3 rightUp = new Vector3(1, 1);

            Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown);
            Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightUp);

            mapWidth = posTwo.x - posOne.x;
            mapHeight = posTwo.y - posOne.y;

            gridWidth = mapWidth / xColumn;
            gridHeight = mapHeight / yRow;

        }
      

        //��ʼ����ͼ
        public void InitMap()
        {
            bigLevelID = 0;
            levelID = 0;
            roundInfoList.Clear();
            bgSR.sprite = null;
            roadSR.sprite = null;
        }

        /// <summary>
        /// ��ȡ�ؿ��ļ�����jsonת��ΪLevelInfo����
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public LevelInfo LoadLevelInfoFile(string fileName)
        {
            LevelInfo levelInfo = new LevelInfo();
            string filePath = Application.dataPath + "/Resources/Json/Level/" + fileName;
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
                return levelInfo;
            }
            Debug.Log("�ļ�����ʧ�ܣ�����·����" + filePath);
            return null;
        }

        /// <summary>
        /// ��ȡLevelInfo��Ϣ��
        /// </summary>
        /// <param name="levelInfo"></param>
        public void LoadLevelInfo(LevelInfo levelInfo)
        {
            bigLevelID = levelInfo.bigLevelID;
            levelID = levelInfo.levelID;
            for (int x = 0; x < xColumn; x++)
            {
                for (int y = 0; y < yRow; y++)
                {
                    gridPoints[x, y].gridState = levelInfo.gridPoints[y + x * yRow];
                    //���¸��ӵ�״̬
                    gridPoints[x, y].UpdateGrid();
                }
            }
            monsterPath.Clear();
            for (int i = 0; i < levelInfo.monsterPath.Count; i++)
            {
                monsterPath.Add(levelInfo.monsterPath[i]);
            }
            roundInfoList = new List<Round.RoundInfo>();
            for (int i = 0; i < levelInfo.roundInfo.Count; i++)
            {
                roundInfoList.Add(levelInfo.roundInfo[i]);
            }
            bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "BG" + (levelID / 3).ToString());
            roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "Road" + levelID);
        }
    }
}

