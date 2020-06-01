using core;

namespace view
{
    class MoveAnimator : IBlockAnimator
    {
        private readonly IAnimatedView view;
        private readonly Coordinate from;
        private readonly Coordinate to;

        public MoveAnimator(IAnimatedView view,
                            Coordinate from,
                            Coordinate to)
        {
            this.view = view;
            this.from = from;
            this.to = to;
        }

        public void RunAnimation()
        {
            view.MoveBlock(from, to);
        }
    }
}
