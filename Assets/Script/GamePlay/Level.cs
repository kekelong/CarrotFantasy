using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class Level
    {
        //回合数
        public int totalRound;
        public Round[] roundList;
        public int currentRound;

        public Level(int roundNum, List<Round.RoundInfo> roundInfoList)
        {
            totalRound = roundNum;
            roundList = new Round[totalRound];
            //对round数组的赋值
            for (int i = 0; i < totalRound; i++)
            {
                roundList[i] = new Round(roundInfoList[i].mMonsterIDList, i, this);
            }
            //设置任务链
            for (int i = 0; i < totalRound; i++)
            {
                if (i == totalRound - 1)
                {
                    break;
                }
                roundList[i].SetNextRound(roundList[i + 1]);
            }
        }

        public void HandleRound()
        {
            if (currentRound >= totalRound)
            {
                GameController.Instance.normalModelpanel.ShowGameWinPage();
            }
            else if (currentRound == totalRound - 1)
            {
                //最后一波怪的UI显示音乐播放
                GameController.Instance.normalModelpanel.ShowFinalWave();
            }
            else
            {
                roundList[currentRound].Handle(currentRound);
            }
        }

        //调用最后一回合的Handle方法
        public void HandleLastRound()
        {
            roundList[currentRound].Handle(currentRound);
        }

        public void AddRoundNum()
        {
            currentRound++;
        }
    }
}
