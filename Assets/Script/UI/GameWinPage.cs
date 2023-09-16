using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Carrot
{
    public class GameWinPage : MonoBehaviour
    {
        public TMP_Text Text_RoundCount;
        public TMP_Text Text_TotalCount;
        public TMP_Text Text_CurrentLevel;
        public NormalModelpanel normalModelpanel;


        private void OnEnable()
        {
            Text_TotalCount.text = normalModelpanel.TotalRound.ToString();
            Text_CurrentLevel.text = (GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5).ToString();
            normalModelpanel.ShowRoundText(Text_RoundCount);
        }

        public void RePlaty()
        {
            normalModelpanel.RePlayGame();
        }

        public void OtherLevel()
        {
            normalModelpanel.SelectOtherLevel();
        }

    }
}
