namespace core
{
    public class MoveResult
    {
        public readonly MoveElementData[] movedElements = null;
        public readonly Coordinate[] removedElements = null;        
        public readonly Block[] spawnedElements = null;

        public MoveResult(MoveElementData[] movedElements)
        {
            this.movedElements = movedElements;
        }

        public MoveResult(MoveElementData[] movedElements,
                          Coordinate[] removedElements,
                          Block[] spawnedElements)
        {
            this.movedElements = movedElements;
            this.removedElements = removedElements;
            this.spawnedElements = spawnedElements;
        }
    }
} // namespace core