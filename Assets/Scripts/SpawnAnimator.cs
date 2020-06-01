namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    class SpawnAnimator : IBlockAnimator
    {
        private IAnimatedView view;
        private Core.Coordinate spawnCoordinate;
        private Core.BlockData blockData;

        public SpawnAnimator(IAnimatedView view,
                             Core.Coordinate spawnCoordinate,
                             Core.BlockData blockData)
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
