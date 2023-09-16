using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Carrot
{
    /// <summary>
    /// �������������ԣ��ӵ������Ĺ�������
    /// </summary>
    public class TowerPersonalProperty : MonoBehaviour
    {

        //����ֵ
        public int towerLevel;//��ǰ���ĵȼ�
        protected float timeVal;//������ʱ��
        public float attackCD;//����CD
        [HideInInspector]
        public int sellPrice;
        [HideInInspector]
        public int upLevelPrice;
        public int price;//��ǰ���ļ۸�

        //��Դ
        protected GameObject bullectGo;//����Դ��Ϊ��ʹ�����Ա�����뷽��

        //����
        [HideInInspector]
        public Tower tower;//�����Լ���tower����
        public Transform targetTrans;//����Ŀ��
        public Animator animator;
        private GameController gameController;

        private Transform muzzle;


        // Use this for initialization
        protected virtual void Start()
        {
            gameController = GameController.Instance;
            upLevelPrice = (int)(price * 1.5f);
            sellPrice = price / 2;
            muzzle = transform.Find("tower");
            animator = muzzle.GetComponent<Animator>();
            timeVal = attackCD;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (gameController.isPause || targetTrans == null || GameController.Instance.gameOver)
            {
                return;
            }
            if (!targetTrans.gameObject.activeSelf)
            {
                targetTrans = null;
                return;
            }
            //����
            if (timeVal >= attackCD / gameController.gameSpeed)
            {
                timeVal = 0;
                Attack();
            }
            else
            {
                timeVal += Time.deltaTime;
            }
            //��ת

            Vector3 WorldPostion = new Vector3(targetTrans.position.x, targetTrans.position.y, transform.position.z);
            transform.LookAt(WorldPostion);
            //if (targetTrans.gameObject.tag == "Item")
            //{
            //    transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
            //}
            //else
            //{
            //    transform.LookAt(targetTrans.position);
            //}

            //if (transform.eulerAngles.y == 0)
            //{
            //    transform.eulerAngles += new Vector3(0, 90, 0);
            //}
        }

        public void Init()
        {
            tower = null;
        }

        /// <summary>
        /// ������
        /// </summary>
        public void SellTower()
        {
            gameController.ChangeCoin(sellPrice);
            GameObject itemGo = gameController.GetGameObjectResource("BuildEffect");
            itemGo.transform.position = transform.position;
            DestoryTower();
        }

        /// <summary>
        /// ����
        /// </summary>
        public void UpLevelTower()
        {
            gameController.ChangeCoin(-upLevelPrice);
            GameObject itemGo = gameController.GetGameObjectResource("UpLevelEffect");
            itemGo.transform.position = transform.position;
            DestoryTower();
        }

        protected virtual void DestoryTower()
        {
            tower.DestoryTower();
        }

        protected virtual void Attack()
        {
            if (targetTrans == null)
            {
                return;
            }
            animator.Play("Attack");
            bullectGo = gameController.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
            bullectGo.transform.position = transform.position;
            bullectGo.GetComponent<Bullect>().targetTrans = targetTrans;
        }
    }
}

