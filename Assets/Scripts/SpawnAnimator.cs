using core;

namespace view
{
    class SpawnAnimator : IBlockAnimator
    {
        private IAnimatedView view;
        private Coordinate spawnCoordinate;
        private BlockData blockData;

        public SpawnAnimator(IAnimatedView view,
                             Coordinate spawnCoordinate,
                             BlockData blockData)
        {
            this.view = view;
            this.spawnCoordinate = spawnCoordinate;
            this.blockData = blockData;
        }

        public void RunAnimation()
        {
            view.SpawnBlock(spawnCoordinate, blockData);
        }
    }
}
