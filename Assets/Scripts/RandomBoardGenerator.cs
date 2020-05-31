using System.Collections.Generic;
using core;

namespace LocalModel
{
    public class RandomBoardGenerator : IBoardProvider
    {
        private const int invalidType = -1;
        private Board board;
        IRandomTypeProvider randomTypeProvider;
        IGameConfig gameConfig;

        public RandomBoardGenerator(IRandomTypeProvider randomTypeProvider, IGameConfig gameConfig)
        {
            this.randomTypeProvider = randomTypeProvider;
            this.gameConfig = gameConfig;
        }

        public Board CreateBoard()
        {
            var rowsCount = gameConfig.RowsCount;
            var columnsCount = gameConfig.ColumnsCount;
            board = new Board(rowsCount, columnsCount);

            for (var row = 0; row < rowsCount; ++row)
            {
                for (var column = 0; column < columnsCount; ++column)
                {
                    var allowedTypesPool = GetAllowedTypesForField(row, column);
                    board[row, column] = randomTypeProvider.GetRandomElementType(allowedTypesPool);
                }
            }

            return board;
        }

        private List<BlockData> GetAllowedTypesForField(int row, int column)
        {
            var toAvoidMatch3Vertically = GetForbiddenTypeForBehindVertical(row, column);
            var toAvoidMatch3Horizontally = GetForbiddenTypeForBehindVertical(row, column);

            var allowedPool = new List<BlockData>();
            foreach (var blockData in gameConfig.BlockDataPool)
            {
                if (blockData.Type == toAvoidMatch3Vertically || blockData.Type == toAvoidMatch3Horizontally)
                {
                    continue;
                }
                allowedPool.Add(blockData);
            }
            return allowedPool;
        }

        private int GetForbiddenTypeForBehindHorizontal(int row, int column)
        {
            if (column < 2)
            {
                return invalidType;
            }
            var twoBehindType = board[row, column].Type;
            var oneBehindType = board[row, column - 1].Type;
            return oneBehindType == twoBehindType ? oneBehindType : invalidType;
        }

        private int GetForbiddenTypeForBehindVertical(int row, int column)
        {
            if (row < 2)
            {
                return invalidType;
            }
            var twoBehindType = board[row - 2, column].Type;
            var oneBehindType = board[row - 1, column].Type;
            return oneBehindType == twoBehindType ? oneBehindType : invalidType;
        }
    }
}
