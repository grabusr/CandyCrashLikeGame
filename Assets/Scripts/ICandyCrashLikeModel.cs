using System.Collections.Generic;
using core;

namespace core
{
    public interface ICandyCrashLikeModel : IGameModelInfo
    {
        List<MoveResult> SwapElements(SwapData swapData);
    }
} // namespace core
