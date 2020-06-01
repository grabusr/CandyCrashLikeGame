using System.Collections.Generic;
using core;

namespace LocalModel
{ 
    public interface IBlockDataProvider
    {
        BlockData GetBlockData();
        BlockData GetBlockDataFromPool(List<BlockData> typesPool);
    }
} // namespace model
