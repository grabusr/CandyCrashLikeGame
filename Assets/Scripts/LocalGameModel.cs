﻿using System.Collections.Generic;
using System;
using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.LocalModel
{
    public class LocalGameModel : IGameModel
    {
        IBlockDataProvider blockProvider;
        private Board board;
        private readonly int minimalElementsCountMatch = 3;

        public LocalGameModel(IBoardProvider boardProvider, IBlockDataProvider blockProvider)
        {
            board = boardProvider.CreateBoard();
            this.blockProvider = blockProvider;
        }

        public int RowsCount => board.RowsCount;
        public int ColumnsCount => board.ColumnsCount;

        public IBoardData Board
        {
            get
            {
                return new BoardData(board);
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

            var moveResults = new List<MoveResult>
            {
                CreateSwapResult(swapData)
            };

            while (matches.Count != 0)
            {
                var movedByGravityElements = board.RemoveBlocks(matches);
                var emptyFields = board.GetEmptyFields();

                var spawnedBlocks = new List<Block>();
                foreach (var field in emptyFields)
                {
                    var blockData = GetNewBlockData();
                    board[field] = blockData;
                    spawnedBlocks.Add(new Block(field, blockData));
                }

                moveResults.Add(CreateMatchDestroyResult(movedByGravityElements,
                                                     matches,
                                                     spawnedBlocks));

                matches = board.GetMatches(minimalElementsCountMatch);
            }

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

        private MoveResult CreateMatchDestroyResult(MoveElementData[] movedElements,
                          List<Coordinate[]> matches,
                          List<Block> spawnedBlocks)
        {
            return new MoveResult(movedElements, matches, spawnedBlocks.ToArray());
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

        private BlockData GetNewBlockData()
        {
            return blockProvider.GetBlockData();
        }
    }
}
