using System.Collections.Generic;

namespace Carrot
{
    public class LevelInfoTool
    {
        public int bigLevelID;
        public int levelID;

        public List<GridPointTool.GridState> gridPoints;

        public List<GridPointTool.GridIndex> monsterPath;

        public List<Round.RoundInfo> roundInfo;
    }
}
