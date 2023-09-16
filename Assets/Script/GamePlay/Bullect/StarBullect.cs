using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Carrot
{
    /// <summary>
    /// 星星子弹
    /// </summary>
    public class StarBullect : MonoBehaviour
    {

        public int attackValue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.activeSelf)
            {
                return;
            }
            if (collision.tag == "Monster" || collision.tag == "Item")
            {
                collision.SendMessage("TakeDamage", attackValue);
            }
        }
    }
}
