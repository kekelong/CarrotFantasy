
using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// 水晶塔
    /// </summary>
    public class Crystal : TowerPersonalProperty
    {

        private float distance;
        private float bullectWidth;
        private float bullectLength;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            bullectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
            bullectGo.SetActive(false);
        }

        private void OnEnable()
        {
            if (animator == null)
            {
                return;
            }
            bullectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
            bullectGo.SetActive(false);
        }

        // Update is called once per frame
        protected override void Update()
        {

            if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.gameOver)
            {
                if (targetTrans == null)
                {
                    bullectGo.SetActive(false);
                }
                return;
            }
            Attack();
        }

        protected override void Attack()
        {
            if (targetTrans == null)
            {
                return;
            }
            animator.Play("Attack");
            distance = Vector3.Distance(transform.position, new Vector3(targetTrans.position.x, targetTrans.position.y,transform.position.z));
            bullectWidth = 3 / distance;
            bullectLength = distance / 2;
            if (bullectWidth <= 0.5f)
            {
                bullectWidth = 0.5f;
            }
            else if (bullectWidth >= 1)
            {
                bullectWidth = 1;
            }
            bullectGo.transform.position = new Vector3((targetTrans.position.x + transform.position.x) / 2, (targetTrans.position.y + transform.position.y) / 2, 0);
            bullectGo.transform.localScale = new Vector3(1, bullectWidth, bullectLength);
            bullectGo.SetActive(true);
            bullectGo.GetComponent<Bullect>().targetTrans = targetTrans;
        }

        protected override void DestoryTower()
        {
            bullectGo.SetActive(false);
            GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString(), bullectGo);
            bullectGo = null;
            base.DestoryTower();
        }
    }

}
