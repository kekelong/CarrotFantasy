using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace Carrot
{
    public class Item : MonoBehaviour
    {
        [HideInInspector]
        public GridPoint gridPoint;
        public int itemID;
        private GameController gameController;
        private int prize;  //销毁金币数额
        private int HP;
        private int currentHP;
        private Slider slider;

        private float timeVal;  //显示或隐藏血条的计时器
        private bool showHp;

        private void OnEnable()
        {
            if (itemID != 0)
            {
                InitItem();
            }
        }

        // Use this for initialization
        void Start()
        {
            gameController = GameController.Instance;
            slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
            InitItem();
            slider.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (showHp)
            {
                if (timeVal <= 0)
                {
                    slider.gameObject.SetActive(false);
                    showHp = false;
                    timeVal = 3;
                }
                else
                {
                    timeVal -= Time.deltaTime;
                }
            }
        }


        private void TakeDamage(int attackValue)
        {
            slider.gameObject.SetActive(true);
            currentHP -= attackValue;
            if (currentHP <= 0)
            {
                DestoryItem();
                return;
            }
            slider.value = (float)currentHP / HP;
            showHp = true;
            timeVal = 3;
        }

        private void DestoryItem()
        {
            if (gameController.targetTrans == transform)
            {
                gameController.HideSignal();
            }

            //金币奖励
            GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas");
            coinGo.transform.Find("Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(gameController.transform);
            coinGo.transform.position = transform.position;

            //销毁特效
            GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect");
            effectGo.transform.SetParent(gameController.transform);
            effectGo.transform.position = transform.position;

            gameController.PushGameObjectToFactory(gameController.mapController.bigLevelID.ToString() + "/Item/" + itemID, gameObject);

            gridPoint.gridState.hasItem = false;
            InitItem();
        }


        private void InitItem()
        {
            prize = 1000 - 100 * itemID;
            HP = 1500 - 100 * itemID;
            currentHP = HP;
            timeVal = 3;
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (gameController.targetTrans == null)
            {
                gameController.targetTrans = transform;
                gameController.ShowSignal();
            }
            //转换新目标
            else if (gameController.targetTrans != transform)
            {
                gameController.HideSignal();
                gameController.targetTrans = transform;
                gameController.ShowSignal();
            }
            //两次点击的是同一个目标
            else if (gameController.targetTrans == transform)
            {
                gameController.HideSignal();
            }
        }
    }
}
