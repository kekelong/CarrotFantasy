using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// 便便子弹
    /// </summary>
    public class TshitBullect : Bullect
    {

        private BullectProperty bullectProperty;

        // Use this for initialization
        void Start()
        {
            bullectProperty = new BullectProperty
            {
                debuffTime = towerLevel * 0.4f,
                debuffValue = towerLevel * 0.15f
            };
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.activeSelf)
            {
                return;
            }
            if (collision.tag == "Monster")
            {
                collision.SendMessage("DecreaseSpeed", bullectProperty);
            }
            base.OnTriggerEnter2D(collision);
        }
    }

    public struct BullectProperty
    {
        public float debuffTime;
        public float debuffValue;
    }
}
