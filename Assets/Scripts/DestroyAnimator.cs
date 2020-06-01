using core;

namespace view
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
