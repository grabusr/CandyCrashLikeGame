namespace core
{
    public interface IGameModelInfo
    {
        int ColumnsCount { get; }
        int RowsCount { get; }
        BlockData[,] Board { get; }
    }

} // namespace core
