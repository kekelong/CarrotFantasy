
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Carrot
{
    public class Round
    {
       
        //回合信息，包含每个回合的monster 
        [System.Serializable]
        public struct RoundInfo
        {
            public int[] mMonsterIDList;
        }
        
        public RoundInfo roundInfo;
        protected Round mNextRound;
        protected int mRoundID;
        protected Level mLevel;

        public Round(int[] monsterIDList, int roundID, Level level)
        {
            mLevel = level;
            roundInfo.mMonsterIDList = monsterIDList;
            mRoundID = roundID;
        }

        public void SetNextRound(Round nextRound)
        {
            mNextRound = nextRound;
        }

        public void Handle(int roundID)
        {
            if (mRoundID < roundID)
            {
                mNextRound.Handle(roundID);
            }
            else
            {
                //产生怪物
                GameController.Instance.mMonsterIDList = roundInfo.mMonsterIDList;
                GameController.Instance.CreateMonster();
            }
        }
    }
}
