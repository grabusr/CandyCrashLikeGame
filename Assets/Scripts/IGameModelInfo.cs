namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface IGameModelInfo
    {
        int ColumnsCount { get; }
        int RowsCount { get; }
        BlockData[,] Board { get; }
    }
}
