using UnityEngine;

using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.UnityView
{
    public class PlayerController : MonoBehaviour, IBoardController
    {
        // TODO use interfaces from inspector instead of implementation types
        [SerializeField] private BoardViewManager gameView;
        [SerializeField] private Config.GameConfig gameConfig;

        private Core.GameController gameController;
        private Element selectedElement;

        public void Start()
        {
            var randomDevice = new LocalModel.SeededRandomTypeProvider(gameConfig.Seed,
                                                                       gameConfig.BlockDataPool);
            var boardProvider = new LocalModel.RandomBoardGenerator(randomDevice, gameConfig);

            var gameModel = new LocalModel.LocalGameModel(boardProvider, randomDevice);

            gameController = new GameController(gameModel);
            gameController.AddGameView(gameView);
        }

        public void OnBlockClicked(Element element)
        {
            if (null == selectedElement)
            {
                selectedElement = element;
                selectedElement.Select();
                return;
            }
            if (selectedElement == element)
            {
                selectedElement.Deselect();
                selectedElement = null;
                return;
            }
            var swapData = new SwapData(selectedElement.Coordinate, element.Coordinate);
            if (gameController.PerformSwap(swapData))
            {
                selectedElement.Deselect();
                selectedElement = null;
            }
        }
    }
}
