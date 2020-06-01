using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    public interface IAnimatedView
    {
        void SpawnBlock(Coordinate coordinate, BlockData blockData);
        void DestroyBlock(Coordinate coordinate);
        void MoveBlock(Coordinate from, Coordinate to);
    }
}
