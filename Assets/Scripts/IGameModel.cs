using System.Collections.Generic;

namespace QuickTurnStudio.CandyCrashLike.Core
{
    public interface IGameModel : IGameModelInfo
    {
        List<MoveResult> SwapElements(SwapData swapData);
    }
}
