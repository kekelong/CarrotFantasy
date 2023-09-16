using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carrot{
    public class GameManager : MonoBehaviour
    {
        #region Manager Instance
        public PlayerManager PlayerManager;
        public FactoryManager FactoryManager;
        public AudioSourceManager AudioSourceManager;
        public UIWindowManager UIWindowManager;
        #endregion

        #region Singleton
        private static GameManager _Instance;
        public static GameManager Instance { get { return _Instance; } }
        #endregion

        public Stage currentStage;
        public bool initPlayManager = false;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _Instance = this;
            PlayerManager = new PlayerManager();
            PlayerManager.ReadData();
            FactoryManager = new FactoryManager();
            AudioSourceManager = new AudioSourceManager();
            UIWindowManager = new UIWindowManager();
            UIWindowManager.mUIFacade.currentWindowState.EnterWindow();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Function
        public GameObject CreateItem(GameObject itemGo)
        {
            GameObject go = Instantiate(itemGo);
            return go;
        }
        //��ȡSprite��Դ
        public Sprite GetSprite(string resourcePath)
        {
            return FactoryManager.spriteFactory.GetSingleResources(resourcePath);
        }

        //��ȡaudioClip��Դ
        public AudioClip GetAudioClip(string resourcePath)
        {
            return FactoryManager.audioClipFactory.GetSingleResources(resourcePath);
        }

        public RuntimeAnimatorController GetRunTimeAnimatorController(string resourcePath)
        {
            return FactoryManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
        }

        //��ȡ��Ϸ����
        public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
        {
            return FactoryManager.factoryDict[factoryType].GetItem(resourcePath);
        }

        //����Ϸ����Żض����
        public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath, GameObject itemGo)
        {
            FactoryManager.factoryDict[factoryType].PushItem(resourcePath, itemGo);
        }
        #endregion
    }
}

