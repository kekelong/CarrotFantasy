using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Carrot
{
    public class MonsterBuilder : IBuilder<Monster>
    {
        public int m_monsterID;
        private GameObject monsterGo;

        public void GetData(Monster productClassGo)
        {
            productClassGo.monsterID = m_monsterID;
            productClassGo.HP = m_monsterID * 100;
            productClassGo.currentHP = productClassGo.HP;
            productClassGo.initMoveSpeed = m_monsterID;
            productClassGo.moveSpeed = m_monsterID * 0.05f + 1.0f;
            productClassGo.prize = m_monsterID * 50;
        }

        public void GetOtherResource(Monster productClassGo)
        {
            productClassGo.GetMonsterProperty();
        }

        public GameObject GetProduct()
        {
            GameObject itemGo = GameController.Instance.GetGameObjectResource("Monster");
            Monster monster = GetProductClass(itemGo);
            GetData(monster);
            GetOtherResource(monster);
            return itemGo;
        }

        public Monster GetProductClass(GameObject gameObject)
        {
            return gameObject.GetComponent<Monster>();
        }
    }

}
