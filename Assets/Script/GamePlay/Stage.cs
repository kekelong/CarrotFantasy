using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    /// <summary>
    /// 游戏关卡信息类
    /// </summary>
    public class Stage
    {
        public int[] mTowerIDList;//本关卡可以建的塔种类

        public int mTowerIDListLength;//建塔数组长度

        public bool mAllClear;//是否清空此关卡道具

        public int mCarrotState; //萝卜状态

        public int mLevelID;//小关卡ID

        public int mBigLevelID;//大关卡ID

        public bool unLocked;//此关卡是否解锁

        public bool mIsRewardLevel;//是否为奖励关卡

        public int mTotalRound;//一共几波怪

        public Stage() { }
        public Stage(int totalRound, int towerIDListLength, int[] towerIDList,
            bool allClear, int carrotState, int levelID, int bigLevelID, bool locked, bool isRewardLevel)
        {
            mTotalRound = totalRound;
            mTowerIDListLength = towerIDListLength;
            mTowerIDList = towerIDList;
            mAllClear = allClear;
            mCarrotState = carrotState;
            mLevelID = levelID;
            mBigLevelID = bigLevelID;
            unLocked = locked;
            mIsRewardLevel = isRewardLevel;
        }
    }
}

