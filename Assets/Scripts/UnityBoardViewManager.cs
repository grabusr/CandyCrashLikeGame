using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using LocalModel;
using view;

namespace view
{
    public class UnityBoardViewManager : MonoBehaviour, core.ICandyCrashLikeView, IBoardView
    {
        [SerializeField]
        private GameConfigComponent gameConfig;

        [SerializeField]
        private Transform elementPrefab;

        private readonly float blockGridSize = 3.0F;

        private List<Element> blockElements;
        private Element firstSelected;

        private IGameModelInfo gameModelInfo;
        private GameController gameController;
        private List<Element> animatedElements = new List<Element>();

        public void Start()
        {
            var randomDevice = new SeededRandomTypeProvider(gameConfig.Seed, gameConfig.BlockDataPool);
            var boardProvider = new RandomBoardGenerator(randomDevice, gameConfig);

            gameController = new GameController(new LocalGameModel(boardProvider, randomDevice));
            gameController.AddGameView(this);
        }

        public void Init(IGameModelInfo gameModelInfo)
        {
            this.gameModelInfo = gameModelInfo;

            var boardData = gameModelInfo.Board;

            blockElements = new List<Element>(gameModelInfo.RowsCount * gameModelInfo.ColumnsCount);

            for (var row = 0; row < gameModelInfo.RowsCount; ++row)
            {
                for (var column = 0; column < gameModelInfo.ColumnsCount; ++column)
                {                
                    blockElements.Add(SpawnElement(new Coordinate(row, column), boardData[row, column]));
                }
            }
        }

        public void OnMoveEvent(MoveResult[] moveResults)
        {
            foreach (var result in moveResults)
            {
                foreach (var moveData in result.movedElements)
                {
                    var elementToAnimate = blockElements.Find(
                        element => element.Coordinate.Equals(moveData.Start));

                    animatedElements.Add(elementToAnimate);
                    elementToAnimate.MoveToPosition(moveData.Destination);
                }
            }
        }

        private int CalculateBlockSize(int rowsCount, int columnsCount)
        {
            var widthFactor = Screen.width / rowsCount;
            var heightFactor = Screen.height / columnsCount;

            return Mathf.Min(widthFactor, heightFactor);
        }
    
        private Element SpawnElement(Coordinate coordinate, BlockData blockData)
        {
            var gameObject = Instantiate(elementPrefab,
                                         GetPositionOfCoordinate(coordinate),
                                         Quaternion.identity,
                                         this.transform);

            var element = gameObject.GetComponent<Element>();
            element.SetGameConfig(gameConfig);
            element.SetBlockData(blockData);            
            element.SetBoardView(this);

            animatedElements.Add(element);
            element.AnimateSpawn();

            return element;
        }

        public void OnBlockSelect(Element selectedElement)
        {
            if (IsAnimating())
            {
                return;
            }
            if (null == firstSelected)
            {
                firstSelected = selectedElement;
                firstSelected.Select();
                return;
            }
            if (firstSelected == selectedElement)
            {
                firstSelected.Deselect();
                firstSelected = null;
                return;
            }
            var swapData = new SwapData(firstSelected.Coordinate, selectedElement.Coordinate);
            if (gameController.PerformSwap(swapData))
            {
                firstSelected.Deselect();
                firstSelected = null;
            }
        }

        public Vector3 GetPositionOfCoordinate(Coordinate coordinate)
        {
            var middleX = blockGridSize * (gameModelInfo.ColumnsCount - 1) * 0.5F;
            var middleY = blockGridSize * (gameModelInfo.RowsCount - 1) * 0.5F;
            return new Vector3(coordinate.Column * blockGridSize - middleX,
                               coordinate.Row * blockGridSize - middleY);
        }

        public void AnimationEnded(Element element)
        {
            animatedElements.Remove(element);
        }

        private bool IsAnimating()
        {
            return animatedElements.Count != 0;
        }
    }
} // namespace view