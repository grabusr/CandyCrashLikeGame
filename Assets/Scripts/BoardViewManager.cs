using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using LocalModel;
using view;

namespace view
{
    public class BoardViewManager : MonoBehaviour, core.ICandyCrashLikeView, IBoardView, IAnimatedView
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Transform elementPrefab;
        [SerializeField] private float blockGridSize = 3.0F;

        private List<Element> elements;
        private List<Element> animatedElements = new List<Element>();
        private List<Element> elementsToDestroy = new List<Element>();
        private Queue<List<IBlockAnimator>> animators = new Queue<List<IBlockAnimator>>();

        private IGameModelInfo gameModelInfo;

        #region overriden from ICandyCrashLikeView

        public void Init(IGameModelInfo gameModelInfo)
        {
            this.gameModelInfo = gameModelInfo;
            var boardData = gameModelInfo.Board;

            elements = new List<Element>(gameModelInfo.RowsCount * gameModelInfo.ColumnsCount);

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

        #endregion

        #region overridden from IBoardView

        public bool IsAnimating()
        {
            return animatedElements.Count != 0 || animators.Count != 0;
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
            if (animatedElements.Count == 0)
            {
                if (AreAnimationsInQueue())
                {
                    RunAnimatorsFromQueue();
                }
                else
                {
                    foreach (var toDestroy in elementsToDestroy)
                    {
                        Destroy(toDestroy);
                    }
                }

            }
        }

        #endregion

        #region overriden from IAnimatedView

        public void SpawnBlock(Coordinate coordinate, BlockData blockData)
        {
            var element = SpawnElement(coordinate, blockData);
            elements.Add(element);

            animatedElements.Add(element);
            element.AnimateSpawn();
        }

        public void DestroyBlock(Coordinate coordinate)
        {
            var element = GetElementAtCoordinate(coordinate);
            if (element == null)
            {
                return;
            }
            animatedElements.Add(element);
            element.AnimateDestroy();
            elements.Remove(element);
        }

        public void MoveBlock(Coordinate from, Coordinate to)
        {
            var element = GetElementAtCoordinate(from);
            animatedElements.Add(element);
            element.MoveToPosition(to);
        }

        #endregion

        private void RunAnimatorsFromQueue()
        {
            var nextAnimators = animators.Dequeue();
            nextAnimators.ForEach(animator => animator.RunAnimation());
        }

        private bool AreAnimationsInQueue()
        {
            return animators.Count != 0;
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
            element.SetPlayerController(playerController);
            element.Coordinate = coordinate;

            return element;
        }

        private Element GetElementAtCoordinate(Coordinate coordinate)
        {
            return elements.Find(element => element.Coordinate.Equals(coordinate));
        }
    }
} // namespace view