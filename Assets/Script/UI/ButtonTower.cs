using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Carrot
{
    /// <summary>
    /// ������ť
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

        //����
        private void BuildTower()
        {
            //�ɽ�����ȥ��������
            gameController.towerBuilder.m_towerID = towerID;
            gameController.towerBuilder.m_towerLevel = 1;
            GameObject towerGo = gameController.towerBuilder.GetProduct();
            towerGo.transform.SetParent(gameController.selectGrid.transform);
            Vector3 po = new Vector3(0, 0, 1);
            towerGo.transform.position = gameController.selectGrid.transform.position + po;
            //������Ч
            GameObject effectGo = gameController.GetGameObjectResource("BuildEffect");
            effectGo.transform.SetParent(gameController.transform);
            effectGo.transform.position = gameController.selectGrid.transform.position;
            //�������
            gameController.selectGrid.AfterBuild();
            gameController.selectGrid.HideGrid();
            gameController.selectGrid.hasTower = true;
            gameController.ChangeCoin(-price);
            //���Ϳջ���ֽ�����ֱ�ӵ��ͬһ�����Ӳ�����ʾ��ť�����
            gameController.selectGrid = null;
            //�òٿػ���������һ�ν�����ֵ�л�
            gameController.handleTowerCanvasGo.SetActive(false);

        }


        //����ͼ��
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

