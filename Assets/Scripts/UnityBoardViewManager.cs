using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using LocalModel;
using view;

namespace view
{
    public class UnityBoardViewManager : MonoBehaviour, core.ICandyCrashLikeView, IBoardView, IAnimatedView
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

        private Queue<List<IBlockAnimator>> animators = new Queue<List<IBlockAnimator>>();

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
                    SpawnBlock(new Coordinate(row, column), boardData[row, column]);
                }
            }
        }

        public void OnMoveEvent(MoveResult[] moveResults)
        {
            foreach (var result in moveResults)
            {
                if (result.removedElements != null
                    && result.removedElements.Count != 0)
                {
                    animators.Enqueue(CreateDestroyAnimators(result));
                }
                if (result.movedElements != null
                    && result.movedElements.Length != 0)
                {
                    animators.Enqueue(CreateMoveAnimators(result));
                }
                if (result.spawnedElements != null
                    && result.spawnedElements.Length != 0)
                {
                    animators.Enqueue(CreateSpawnAnimators(result));
                }
            }
            RunAnimatorsFromQueue();
        }

        private void RunAnimatorsFromQueue()
        {
            var nextAnimators = animators.Dequeue();
            nextAnimators.ForEach(animator => animator.RunAnimation());
        }

        private List<IBlockAnimator> CreateSpawnAnimators(MoveResult result)
        {
            var spawnAnimators = new List<IBlockAnimator>();
            foreach (var block in result.spawnedElements)
            {
                spawnAnimators.Add(new SpawnAnimator(this, block.Coord, block.BlockData));
            }

            return spawnAnimators;
        }

        private List<IBlockAnimator> CreateDestroyAnimators(MoveResult result)
        {
            var removeAnimators = new List<IBlockAnimator>();
            foreach (var match in result.removedElements)
            {
                foreach (var block in match)
                {
                    removeAnimators.Add(new DestroyAnimator(this, block));
                }
            }

            return removeAnimators;
        }

        private List<IBlockAnimator> CreateMoveAnimators(MoveResult result)
        {
            var moveAnimators = new List<IBlockAnimator>();
            foreach (var move in result.movedElements)
            {
                moveAnimators.Add(new MoveAnimator(this, move.Start, move.Destination));
            }

            return moveAnimators;
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
            element.Coordinate = coordinate;

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
            if (animatedElements.Count == 0 && animators.Count != 0)
            {
                RunAnimatorsFromQueue();
            }
        }

        private bool IsAnimating()
        {
            return animatedElements.Count != 0 || animators.Count != 0;
        }

        public void SpawnBlock(Coordinate coordinate, BlockData blockData)
        {
            var element = SpawnElement(coordinate, blockData);
            blockElements.Add(element);

            animatedElements.Add(element);
            element.AnimateSpawn();
        }

        public void DestroyBlock(Coordinate coordinate)
        {
            var element = GetElementAtCoordinate(coordinate);
            animatedElements.Add(element);
            element.AnimateDestroy();
            blockElements.Remove(element);
        }

        public void MoveBlock(Coordinate from, Coordinate to)
        {
            var element = GetElementAtCoordinate(from);
            animatedElements.Add(element);
            element.MoveToPosition(to);
        }

        private Element GetElementAtCoordinate(Coordinate coordinate)
        {
            return blockElements.Find(element => element.Coordinate.Equals(coordinate));
        }
    }
} // namespace view