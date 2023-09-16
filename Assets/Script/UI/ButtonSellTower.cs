using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    /// <summary>
    /// 卖塔按钮
    /// </summary>
    public class ButtonSellTower : MonoBehaviour
    {

        private int price;
        private Button button;
        private TMP_Text text;
        private GameController gameController;

        // Use this for initialization
        void Start()
        {
            gameController = GameController.Instance;
            button = GetComponent<Button>();
            text = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            button.onClick.AddListener(SellTower);
        }

        private void OnEnable()
        {
            if (text == null)
            {
                return;
            }
            price = gameController.selectGrid.towerPersonalProperty.sellPrice;
            text.text = price.ToString();
        }

        private void SellTower()
        {
            gameController.selectGrid.towerPersonalProperty.SellTower();
            gameController.selectGrid.InitGrid();
            gameController.selectGrid.handleTowerGanvasGo.SetActive(false);
            gameController.selectGrid.HideGrid();
            gameController.selectGrid = null;
        }
    }

}
