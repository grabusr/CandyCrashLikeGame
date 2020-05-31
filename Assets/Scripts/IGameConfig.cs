using System.Collections.Generic;

namespace LocalModel
{
    public interface IGameConfig
    {
        int RowsCount { get; }
        int ColumnsCount { get; }
        core.BlockData[] BlockDataPool { get; }
        int Seed { get; }
    }
}
