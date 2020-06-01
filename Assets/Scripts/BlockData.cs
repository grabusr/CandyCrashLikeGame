namespace QuickTurnStudio.CandyCrashLike.Core
{
    public struct BlockData
    {
        public static readonly int invalidColorId = -1;

        public int Type { get; }

        public BlockData(int type)
        {
            this.Type = type;
        }

        
    }
}
