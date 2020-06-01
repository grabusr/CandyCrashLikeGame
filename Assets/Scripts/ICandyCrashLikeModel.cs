using System.Collections.Generic;

namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface ICandyCrashLikeModel : IGameModelInfo
    {
        List<MoveResult> SwapElements(SwapData swapData);
    }
}
