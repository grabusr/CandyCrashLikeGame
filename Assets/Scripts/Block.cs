namespace QuickTurnStudio.CandyCrashLike.Core
{
    public struct Block
    {
        public Coordinate Coord { get; }
        public BlockData BlockData { get; }

        public Block(Coordinate coordinate, BlockData blockData)
        {
            this.Coord = coordinate;
            this.BlockData = blockData;
        }

        public int Column
        {
            get => Coord.Column;
        }

        public int Row
        {
            get => Coord.Row;
        }
    }
}
