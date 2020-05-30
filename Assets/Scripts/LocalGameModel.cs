using System.Collections.Generic;
using System;
using core;

namespace LocalModel
{

    public class LocalGameModel : ICandyCrashLikeModel
    {
        private Board board;

        public LocalGameModel(IBoardProvider boardProvider)
        {
            board = boardProvider.CreateBoard();
        }

        public int BoardWidth => board.RowsCount;
        public int BoardHeight => board.ColumnsCount;

        public MoveResult[] SwapElements(SwapData swapData)
        {
            if (!IsValidCoordinate(swapData.Position1)
                || !IsValidCoordinate(swapData.Position2)
                || !AreCoordinateNeighbours(swapData.Position1, swapData.Position2))
            {
                return null;
            }           

            board.SwapFields(swapData.Position1, swapData.Position2);

            var moveResults = new List<MoveResult>
            {
                CreateSwapResult(swapData)
            };

            return moveResults.ToArray();
        }

        private MoveResult CreateSwapResult(SwapData swapData)
        {
            var movesData = new MoveElementData[]{
                new MoveElementData(swapData.Position1, swapData.Position2),
                new MoveElementData(swapData.Position2, swapData.Position1),
            };

            return new MoveResult(movesData);
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.Column > 0 && coordinate.Column < board.ColumnsCount
                && coordinate.Row > 0 && coordinate.Row < board.RowsCount;
        }

        private bool AreCoordinateNeighbours(Coordinate coordinate1, Coordinate coordinate2)
        {
            return Math.Abs(coordinate1.Row - coordinate2.Row)
                + Math.Abs(coordinate1.Column - coordinate2.Column) == 1;
        }
    }

} // namespace LocalModel