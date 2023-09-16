using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class LevelInfo
    {
        public int bigLevelID;
        public int levelID;

        public List<GridPoint.GridState> gridPoints;

        public List<GridPoint.GridIndex> monsterPath;

        public List<Round.RoundInfo> roundInfo;
    }
}
