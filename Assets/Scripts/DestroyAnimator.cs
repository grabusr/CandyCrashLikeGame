using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    class DestroyAnimator : IBlockAnimator
    {
        private IAnimatedView view;
        private Coordinate blockCoordinate;

        public DestroyAnimator(IAnimatedView view,
                               Coordinate blockCoordinate)
        {
            this.view = view;
            this.blockCoordinate = blockCoordinate;
        }

        public void RunAnimation()
        {
            view.DestroyBlock(blockCoordinate);
        }
    }
}
