using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;

namespace Carrot
{
    public class PlayerManager
    {
        public int adventrueModelNum; //冒险模式解锁的地图个数
        public int burriedLevelNum; //隐藏关卡解锁的地图个数
        public int bossModelNum;//boss模式KO的BOSS
        public int coin;//获得金币的总数
        public int killMonsterNum;//杀怪总数
        public int killBossNum;//杀掉BOSS的总数
        public int clearItemNum;//清理道具的总数

        public List<bool> unLockedNormalModelBigLevelList;//大关卡
        public List<Stage> unLockedNormalModelLevelList; //所有的小关卡
        public List<int> unLockedeNormalModelLevelNum; //解锁小关卡数量

        //怪物窝
        public int cookies;
        public int milk;
        public int nest;
        public int diamands;

        public PlayerManager()
        {
            adventrueModelNum = 100;
            burriedLevelNum = 100;
            bossModelNum = 100;
            coin = 100;
            killMonsterNum = 100;
            killBossNum = 100;
            clearItemNum = 100;
            unLockedeNormalModelLevelNum = new List<int>()
            {
                5,5,5
            };
            unLockedNormalModelBigLevelList = new List<bool>()
            {
                true,true,true
            };
            unLockedNormalModelLevelList = new List<Stage>()
            {
                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,1,1,true,false),
                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,2,1,true,false),
                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,3,1,true,false),
                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,4,1,true,false),
                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,5,1,true,true),

                new Stage(10,5,new int[]{ 1,2,3,4,5},false,0,1,2,true,false),
                new Stage(10,5, new int[] { 1, 2, 3, 4, 5 },false,0,2,2,true,false),
                new Stage(10,5, new int[] { 1, 2, 3, 4, 5 },false,0,3,2,true,false),
                new Stage(10,5, new int[] { 1, 2, 3, 4, 5 },false,0,4,2,true,false),
                new Stage(10,5, new int[] { 1, 2, 3, 4, 5 },false,0,5,2,true,false),

                new Stage(10,8,new int[]{1, 2, 3, 4, 5, 6, 7,8 },false,0,1,3,true,false),
                new Stage(10,9,new int[]{1, 2, 3, 4, 5, 6, 7 ,8,9},false,0,2,3,true,false),
                new Stage(10,10,new int[]{1, 2, 3, 4, 5, 6, 7 ,8,9,10},false,0,3,3,true,false),
                new Stage(10,11,new int[]{1, 2, 3, 4, 5, 6, 7 ,8,9,10,11},false,0,4,3,true,false),
                new Stage(10,12,new int[]{1, 2, 3, 4, 5, 6, 7,8,9,10,11,12 },false,0,5,3,true,false),
            };
        }



        public void SaveData()
        {
            Memento memento = new Memento();
            memento.SaveByJson();
        }

        public void ReadData()
        {
            Memento memento = new Memento();
            PlayerManager playerManager = memento.LoadByJson();
            adventrueModelNum = playerManager.adventrueModelNum;
            burriedLevelNum = playerManager.burriedLevelNum;
            bossModelNum = playerManager.bossModelNum;//boss模式KO的BOSS
            coin = playerManager.coin;//获得金币的总数
            killMonsterNum = playerManager.killMonsterNum;//杀怪总数
            killBossNum = playerManager.killBossNum;//杀掉BOSS的总数
            clearItemNum = playerManager.clearItemNum;//清理道具的总数
            unLockedNormalModelBigLevelList = playerManager.unLockedNormalModelBigLevelList;//大关卡
            unLockedNormalModelLevelList = playerManager.unLockedNormalModelLevelList; //所有的小关卡        }
            unLockedeNormalModelLevelNum = playerManager.unLockedeNormalModelLevelNum; //解锁小关卡数量    }
        }
    }
}
