using Carrot;
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
    /// 升级按钮
    /// </summary>
    public class ButtonUpLevel : MonoBehaviour
    {

        private int price;
        private Button button;
        private TMP_Text text;
        private Image image;
        private Sprite canUpLevelSprite;
        private Sprite cantUpLevelSprite;
        private Sprite reachHighestLevel;
        private GameController gameController;

        private void OnEnable()
        {
            if (text == null)
            {
                return;
            }
            UpdateUIView();
        }

        // Use this for initialization
        void Start()
        {
            gameController = GameController.Instance;
            button = GetComponent<Button>();
            button.onClick.AddListener(UpLevel);
            canUpLevelSprite = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
            cantUpLevelSprite = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
            reachHighestLevel = gameController.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
            text = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            image = GetComponent<Image>();
        }

        private void UpdateUIView()
        {
            if (gameController.selectGrid.towerLevel >= 3)
            {
                image.sprite = reachHighestLevel;
                button.interactable = false;
                text.enabled = false;
            }
            else
            {
                text.enabled = true;
                price = gameController.selectGrid.towerPersonalProperty.upLevelPrice;
                text.text = price.ToString();
                if (gameController.coin >= price)
                {
                    image.sprite = canUpLevelSprite;
                    button.interactable = true;
                }
                else
                {
                    image.sprite = cantUpLevelSprite;
                    button.interactable = false;
                }
            }
        }

        private void UpLevel()
        {
            //赋值建造要产生的塔的属性
            gameController.towerBuilder.m_towerID = gameController.selectGrid.tower.towerID;
            gameController.towerBuilder.m_towerLevel = gameController.selectGrid.towerLevel + 1;
            //销毁之前的等级的塔
            gameController.selectGrid.towerPersonalProperty.UpLevelTower();
            //产生新塔
            GameObject towerGo = gameController.towerBuilder.GetProduct();
            towerGo.transform.SetParent(gameController.selectGrid.transform);
            Vector3 po = new Vector3(0, 0, 1);
            towerGo.transform.position = gameController.selectGrid.transform.position + po;
            gameController.selectGrid.AfterBuild();
            gameController.selectGrid.HideGrid();
            gameController.selectGrid = null;
        }
    }
}
