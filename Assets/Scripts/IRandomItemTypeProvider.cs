using System.Collections.Generic;

namespace QuickTurnStudio.CandyCrashLike.LocalModel
{ 
    public interface IBlockDataProvider
    {
        Core.BlockData GetBlockData();
        Core.BlockData GetBlockDataFromPool(List<Core.BlockData> typesPool);
    }
}
