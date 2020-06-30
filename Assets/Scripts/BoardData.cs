using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.LocalModel
{
    public class BoardData : IBoardData
    {
        private Board board;

        public int RowsCount { get { return board.RowsCount; } }
        public int ColumnsCount { get { return board.ColumnsCount; } }

        public BoardData(Board board)
        {
            this.board = board;
        }

        public BlockData this[int row, int column]
        {
            get => board[row, column];
        }
    }
}