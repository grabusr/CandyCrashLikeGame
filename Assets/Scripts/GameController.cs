using System.Collections.Generic;

namespace QuickTurnStudio.CandyCrashLike.Core
{
    public class GameController
    {
        private IGameModel gameModel;
        private List<IGameView> gameViews = new List<IGameView>();

        public GameController(IGameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void AddGameView(IGameView view)
        {
            if (null == view)
            {
                return;
            }
            view.Init(gameModel);
            gameViews.Add(view);
        }

        public bool PerformSwap(SwapData swap)
        {
            var swapResults = gameModel.SwapElements(swap);
            if (null == swapResults)
            {
                return false;
            }

            InformViewsAboutMoveResult(swapResults);

            return true;
        }

        private void InformViewsAboutMoveResult(List<MoveResult> moveResults)
        {
            foreach (var view in gameViews)
            {
                view.OnMoveEvent(moveResults.ToArray());
            }
        }
    }
}
