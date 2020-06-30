namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface IBoardData
    {
        int RowsCount { get; }
        int ColumnsCount { get; }
        BlockData this[int row, int column] { get; }
    }
}