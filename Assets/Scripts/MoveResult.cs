using System.Collections.Generic;

namespace QuickTurnStudio.CandyCrashLike.Core
{
    public class MoveResult
    {
        public readonly MoveElementData[] movedElements = null;
        public readonly List<Coordinate[]> removedElements = null;        
        public readonly Block[] spawnedElements = null;

        public MoveResult(MoveElementData[] movedElements)
        {
            this.movedElements = movedElements;
        }

        public MoveResult(MoveElementData[] movedElements,
                          List<Coordinate[]> removedElements,
                          Block[] spawnedElements)
        {
            this.movedElements = movedElements;
            this.removedElements = removedElements;
            this.spawnedElements = spawnedElements;
        }
    }
}
