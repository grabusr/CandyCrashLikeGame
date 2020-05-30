using System;

using core;

namespace LocalModel
{
    public class Board
    {
        private BlockData[,] fields;
        private int rowsCount;
        private int columnsCount;

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
    }
} // namespace LocalModel
