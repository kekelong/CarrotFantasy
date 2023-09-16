using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    /// <summary>
    /// 建塔按钮
    /// </summary>
    public class ButtonTower : MonoBehaviour
    {

        public int towerID;
        public int price;
        private Button button;
        private Sprite canClickSprite;
        private Sprite cantClickSprite;
        private Image image;
        private GameController gameController;

        private void OnEnable()
        {
            if (price == 0)
            {
                return;
            }
            UpdateIcon();
        }

        // Use this for initialization
        void Start()
        {
            gameController = GameController.Instance;
            image = GetComponent<Image>();
            button = GetComponent<Button>();
            canClickSprite = gameController.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
            cantClickSprite = gameController.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");
            UpdateIcon();
            price = gameController.towerPriceDict[towerID];
            button.onClick.AddListener(BuildTower);
        }

        //建塔
        private void BuildTower()
        {
            //由建塔者去建造新塔
            gameController.towerBuilder.m_towerID = towerID;
            gameController.towerBuilder.m_towerLevel = 1;
            GameObject towerGo = gameController.towerBuilder.GetProduct();
            towerGo.transform.SetParent(gameController.selectGrid.transform);
            Vector3 po = new Vector3(0, 0, 1);
            towerGo.transform.position = gameController.selectGrid.transform.position + po;
            //产生特效
            GameObject effectGo = gameController.GetGameObjectResource("BuildEffect");
            effectGo.transform.SetParent(gameController.transform);
            effectGo.transform.position = gameController.selectGrid.transform.position;
            //处理格子
            gameController.selectGrid.AfterBuild();
            gameController.selectGrid.HideGrid();
            gameController.selectGrid.hasTower = true;
            gameController.ChangeCoin(-price);
            //不滞空会出现建完塔直接点击同一个格子不会显示按钮的情况
            gameController.selectGrid = null;
            //让操控画布先隐藏一次进行数值切换
            gameController.handleTowerCanvasGo.SetActive(false);

        }


        //更新图标
        private void UpdateIcon()
        {
            if (gameController.coin > price)
            {
                image.sprite = canClickSprite;
                button.interactable = true;
            }
            else
            {
                image.sprite = cantClickSprite;
                button.interactable = false;
            }
        }
    }
}

