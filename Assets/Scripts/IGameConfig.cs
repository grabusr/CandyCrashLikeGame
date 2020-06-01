namespace QuickTurnStudio.CandyCrashLike.LocalModel
{
    public interface IGameConfig
    {
        int RowsCount { get; }
        int ColumnsCount { get; }
        Core.BlockData[] BlockDataPool { get; }
        int Seed { get; }
    }
}
