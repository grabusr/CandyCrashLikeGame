using System.Collections.Generic;
using core;

namespace LocalModel
{ 
    public interface IRandomTypeProvider
    {
        BlockData GetRandomElementType(List<BlockData> typesPool);
    }
} // namespace model
