using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Carrot
{
    public class Monster : MonoBehaviour
    {
        //属性值
        public int monsterID;
        public int HP;//总血量
        public int currentHP;//当前血量
        public float moveSpeed;//当前速度
        public float initMoveSpeed;//初始速度
        public int prize;//奖励金钱

        //引用
        private Animator animator;
        private Slider slider;
        private GameController gameController;
        private List<Vector3> monsterPointList;

        //用于计数的属性或开关
        private int roadPointIndex = 1;
        private bool reachCarrot;//到达终点
        private bool hasDecreasSpeed;//是否减速

        private float decreaseSpeedTimeVal;//减速计时器
        private float decreaseTime;//减速持续的具体时间

        //资源
        public AudioClip dieAudioClip;
        public RuntimeAnimatorController runtimeAnimatorController;
        private GameObject TshitGo;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            slider = transform.Find("Canvas").Find("Slider").GetComponent<Slider>();
            slider.gameObject.SetActive(false);
            gameController = GameController.Instance;
            monsterPointList = GameController.Instance.mapController.monsterPathPos;
            TshitGo = transform.Find("TShit").gameObject;
        }

        private void OnEnable()
        {
            gameController = GameController.Instance;
            //怪物的朝向
            if (roadPointIndex + 1 < monsterPointList.Count)
            {
                float xOffset = monsterPointList[0].x - monsterPointList[1].x;
                if (xOffset < 0)//右走
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (xOffset > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }

        private void Update()
        {
            if (gameController.isPause || gameController.gameOver)
            {
                return;
            }
            if (!reachCarrot)
            {
                transform.position = Vector3.Lerp(transform.position, monsterPointList[roadPointIndex],
                    1 / Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) * Time.deltaTime * moveSpeed * gameController.gameSpeed);
                if (Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) <= 0.0001f)
                {
                    //怪物的转向
                    if (roadPointIndex + 1 < monsterPointList.Count)
                    {
                        float xOffset = monsterPointList[roadPointIndex].x - monsterPointList[roadPointIndex + 1].x;
                        if (xOffset < 0)//右走
                        {
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else if (xOffset > 0)
                        {
                            transform.eulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                    slider.gameObject.transform.eulerAngles = Vector3.zero;
                    roadPointIndex++;
                    if (roadPointIndex >= gameController.mapController.monsterPathPos.Count)
                    {
                        reachCarrot = true;
                    }
                }
            }
            else//到达终点
            {
                DestoryMonster();
                //萝卜减血
                gameController.DecreaseHP();
            }

            if (hasDecreasSpeed)
            {
                decreaseSpeedTimeVal += Time.deltaTime;
            }
            if (decreaseSpeedTimeVal >= decreaseTime)
            {
                CancelDecreaseDebuff();
                decreaseSpeedTimeVal = 0;
            }
        }

        //销毁怪物
        private void DestoryMonster()
        {
            
            if (!reachCarrot)//被玩家杀死的
            {
                //生成金币以及数目
                GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas");
                coinGo.transform.Find("Coin").GetComponent<CoinMove>().prize = prize;
                coinGo.transform.SetParent(gameController.transform);
                coinGo.transform.position = transform.position;
                //增加玩家的金币数量
                gameController.ChangeCoin(prize);
            }
            //产生销毁特效
            GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect");
            effectGo.transform.SetParent(gameController.transform);
            effectGo.transform.position = transform.position;
            gameController.killMonsterNum++;
            gameController.killMonsterTotalNum++;
            Transform target = transform.Find("TargetSignal");
            if (target)
            {
                target.gameObject.SetActive(false);
                target.transform.SetParent(gameController.transform);
            }
            InitMonsterGo();
            gameController.PushGameObjectToFactory("Monster", gameObject);

        }

        /// <summary>
        /// 初始化怪物的方法
        /// </summary>
        private void InitMonsterGo()
        {
            monsterID = 0;
            HP = 0;
            currentHP = 0;
            moveSpeed = 0;
            roadPointIndex = 1;
            dieAudioClip = null;
            reachCarrot = false;
            slider.value = 1;
            slider.gameObject.SetActive(false);
            prize = 0;
            transform.eulerAngles = new Vector3(0, 0, 0);
            decreaseSpeedTimeVal = 0;
            decreaseTime = 0;
            //CancelDecreaseDebuff();
        }

        /// <summary>
        /// 承受伤害的方法
        /// </summary>
        private void TakeDamage(int attackValue)
        {
            slider.gameObject.SetActive(true);
            currentHP -= attackValue;
            if (currentHP < 0)
            {
                //播放死亡音效
                DestoryMonster();
                return;
            }
            slider.value = (float)currentHP / HP;
        }

        //减速buff的方法
        private void DecreaseSpeed(BullectProperty bullectProperty)
        {
            if (!hasDecreasSpeed)
            {
                moveSpeed = moveSpeed * bullectProperty.debuffValue;
                TshitGo.SetActive(true);
            }
            decreaseSpeedTimeVal = 0;
            hasDecreasSpeed = true;
            decreaseTime = bullectProperty.debuffTime;
        }

        /// <summary>
        /// 用来取消减速buff的方法
        /// </summary>
        private void CancelDecreaseDebuff()
        {
            hasDecreasSpeed = false;
            moveSpeed = initMoveSpeed;
            TshitGo.SetActive(false);
        }

        //获取特异性属性的方法
        public void GetMonsterProperty()
        {
            runtimeAnimatorController = gameController.controllers[monsterID - 1];
            animator.runtimeAnimatorController = runtimeAnimatorController;
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
