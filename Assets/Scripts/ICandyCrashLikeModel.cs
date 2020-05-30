using System.Collections.Generic;
using core;

namespace core
{
    public interface ICandyCrashLikeModel : IGameModelInfo
    {
        MoveResult[] SwapElements(SwapData swapData);
    }
} // namespace core
