using LitJson;
using System.IO;
using UnityEngine;

namespace Carrot
{
    public class Memento
    {
        //∂¡»°
        public void SaveByJson()
        {
            PlayerManager playerManager = GameManager.Instance.PlayerManager;
            string path = Application.dataPath + "/Resources/Json/" + "playerManager.json";

            string json = JsonMapper.ToJson(playerManager);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
            writer.Close();
        }

        //¥Ê¥¢

        public PlayerManager LoadByJson()
        {
            PlayerManager playerManager = new PlayerManager();
            string path = "";
            if(!GameManager.Instance.initPlayManager)
            {
                path = Application.dataPath + "/Resources/Json/" + "playerManager.json";
            }
            else
            {
                path = Application.dataPath + "/Resources/Json/" + "playerManagerInit.json";
            }
            if(File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string json = reader.ReadToEnd();
                reader.Close();
                playerManager = JsonMapper.ToObject<PlayerManager>(json);
            }
            else
            {
                Debug.Log("exit path file");
                Debug.Log(path);
            }
            return playerManager;

        }
    }
}
