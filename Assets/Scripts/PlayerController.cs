using UnityEngine;

using core;
using LocalModel;

namespace view
{
    public class PlayerController : MonoBehaviour, IBoardController
    {
        // TODO use interfaces from inspector instead of implementation types
        [SerializeField] private BoardViewManager gameView;
        [SerializeField] private GameConfig gameConfig;

        private core.GameController gameController;
        private Element selectedElement;

        public void Start()
        {
            var randomDevice = new SeededRandomTypeProvider(gameConfig.Seed, gameConfig.BlockDataPool);
            var boardProvider = new RandomBoardGenerator(randomDevice, gameConfig);

            var gameModel = new LocalGameModel(boardProvider, randomDevice);

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
