﻿using System.Collections.Generic;

using core;

namespace LocalModel
{
    public class Board
    {
        private BlockData[,] fields;
        private readonly int rowsCount;
        private readonly int columnsCount;

        public Board(int rowsCount, int columnsCount)
        {
            fields = new BlockData[rowsCount, columnsCount];
            this.rowsCount = rowsCount;
            this.columnsCount = columnsCount;
        }

        public BlockData this[int row, int column]
        {
            get => fields[row, column];
            set => fields[row, column] = value;
        }

        public BlockData this[Coordinate coordinate]
        {
            get => fields[coordinate.Row, coordinate.Column];
            set => fields[coordinate.Row, coordinate.Column] = value;
        }

        public int RowsCount
        {
            get => rowsCount;
        }

        public int ColumnsCount
        {
            get => columnsCount;
        }

        public void SwapFields(Coordinate coord1, Coordinate coord2)
        {
            var tempBlock = fields[coord1.Row, coord1.Column];
            fields[coord1.Row, coord1.Column] = fields[coord2.Row, coord2.Column];
            fields[coord2.Row, coord2.Column] = tempBlock;
        }

        public List<Coordinate[]> GetMatches(int minimalBlockCount)
        {
            var matches = GetHorizontalMatches(minimalBlockCount);
            matches.AddRange(GetVerticalMatches(minimalBlockCount));
            return matches;
        }

        public List<Coordinate[]> GetVerticalMatches(int minimalBlockCount)
        {
            var matches = new List<Coordinate[]>();

            for (var row = 0; row < RowsCount; ++row)
            {
                var verticalMatch = new List<Coordinate>();
                var type = fields[row, 0].Type;
                for (var column = 0; column < ColumnsCount; ++column)
                {
                    var nextType = fields[row, column].Type;
                    if (nextType == type)
                    {
                        verticalMatch.Add(new Coordinate(row, column));
                    }
                    else
                    {
                        if (verticalMatch.Count >= minimalBlockCount)
                        {
                            matches.Add(verticalMatch.ToArray());
                        }
                        verticalMatch.Clear();
                    }
                }
                if (verticalMatch.Count >= minimalBlockCount)
                {
                    matches.Add(verticalMatch.ToArray());
                }
            }
            return matches;
        }

        public List<Coordinate[]> GetHorizontalMatches(int minimalBlockCount)
        {
            var matches = new List<Coordinate[]>();

            for (var column = 0; column < ColumnsCount; ++column)
            {
                var horizontalMatch = new List<Coordinate>();
                var type = fields[0, column].Type;
                for (var row = 0; row < RowsCount; ++row)
                {
                    var nextType = fields[row, column].Type;
                    if (horizontalMatch.Count == 0)
                    {
                        type = nextType;
                    }
                    if (nextType == type)
                    {
                        horizontalMatch.Add(new Coordinate(row, column));
                    }
                    else
                    {
                        if (horizontalMatch.Count >= minimalBlockCount)
                        {
                            matches.Add(horizontalMatch.ToArray());
                        }
                        horizontalMatch.Clear();
                    }
                }
                if (horizontalMatch.Count >= minimalBlockCount)
                {
                    matches.Add(horizontalMatch.ToArray());
                }
            }
            return matches;
        }

        public MoveElementData[] RemoveBlocks(List<Coordinate[]> matches)
        {
            MarkBlockAsRemoved(matches);
            return ApplayGravity().ToArray();
        }

        private List<MoveElementData> ApplayGravity()
        {
            var moveElementData = new List<MoveElementData>();
            var rowsToProcess = RowsCount - 1;
            for (var row = 0; row < rowsToProcess; ++row)
            {
                for (var column = 0; column < ColumnsCount; ++column)
                {
                    if (fields[row, column].Type != BlockData.invalidColorId)
                    {
                        continue;
                    }
                    var emptyPosition = new Coordinate(row, column);
                    var swappedPosition = new Coordinate(row + 1, column);
                    SwapFields(emptyPosition, swappedPosition);
                    moveElementData.Add(new MoveElementData(swappedPosition, emptyPosition));
                }
            }
            return moveElementData;
        }

        private void MarkBlockAsRemoved(List<Coordinate[]> matches)
        {
            foreach (var match in matches)
            {
                foreach (var coord in match)
                {
                    this[coord] = new BlockData(BlockData.invalidColorId);
                }
            }
        }
    }
} // namespace LocalModel
