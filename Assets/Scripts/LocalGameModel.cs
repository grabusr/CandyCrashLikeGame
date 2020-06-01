using System.Collections.Generic;
using System;
using core;

namespace LocalModel
{

    public class LocalGameModel : ICandyCrashLikeModel
    {
        private Board board;
        private readonly int minimalElementsCountMatch = 3;

        public LocalGameModel(IBoardProvider boardProvider)
        {
            board = boardProvider.CreateBoard();
        }

        public int RowsCount => board.RowsCount;
        public int ColumnsCount => board.ColumnsCount;

        public BlockData[,] Board
        {
            get
            {
                var blockDataBoard = new BlockData[RowsCount, ColumnsCount];

                for (var row = 0; row < RowsCount; ++row)
                {
                    for (var column = 0; column < ColumnsCount; ++column)
                    {
                        blockDataBoard[row, column] = board[row, column];
                    }
                }

                return blockDataBoard;
            }
        }

        public List<MoveResult> SwapElements(SwapData swapData)
        {
            if (!IsValidCoordinate(swapData.First)
                || !IsValidCoordinate(swapData.Second)
                || !AreCoordinateNeighbours(swapData.First, swapData.Second))
            {
                return null;
            }           

            board.SwapFields(swapData.First, swapData.Second);

            var matches = board.GetMatches(minimalElementsCountMatch);
            if (matches.Count == 0)
            {
                board.SwapFields(swapData.First, swapData.Second);
                return null;
            }


            var swapResult = CreateSwapResult(swapData);

            var moveResults = new List<MoveResult>();
            moveResults.Add(swapResult);

            return moveResults;
        }

        private MoveResult CreateSwapResult(SwapData swapData)
        {   
            var movesData = new MoveElementData[]{
                new MoveElementData(swapData.First, swapData.Second),
                new MoveElementData(swapData.Second, swapData.First),
            };

            return new MoveResult(movesData);
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.Column >= 0 && coordinate.Column < board.ColumnsCount
                && coordinate.Row >= 0 && coordinate.Row < board.RowsCount;
        }

        private bool AreCoordinateNeighbours(Coordinate coordinate1, Coordinate coordinate2)
        {
            return Math.Abs(coordinate1.Row - coordinate2.Row)
                + Math.Abs(coordinate1.Column - coordinate2.Column) == 1;
        }
    }

} // namespace LocalModel