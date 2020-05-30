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

        RandomBoardGenerator(IRandomTypeProvider randomTypeProvider, IGameConfig gameConfig)
        {
            this.randomTypeProvider = randomTypeProvider;
            this.gameConfig = gameConfig;
        }

        public Board CreateBoard()
        {
            var rowsCount = gameConfig.BoardWidth;
            var columnsCount = gameConfig.BoardHeight;
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
            foreach (var type in gameConfig.TypesPool)
            {
                if (type == toAvoidMatch3Vertically || type == toAvoidMatch3Horizontally)
                {
                    continue;
                }
                allowedPool.Add(new BlockData(type));
            }
            return allowedPool;
        }

        private int GetForbiddenTypeForBehindHorizontal(int column, int row)
        {
            if (column < 2)
            {
                return invalidType;
            }
            var twoBehindType = board[column - 2, row].Type;
            var oneBehindType = board[column - 1, row].Type;
            return oneBehindType == twoBehindType ? oneBehindType : invalidType;
        }

        private int GetForbiddenTypeForBehindVertical(int row, int column)
        {
            if (row < 2)
            {
                return invalidType;
            }
            var twoBehindType = board[column, row - 2].Type;
            var oneBehindType = board[column, row - 1].Type;
            return oneBehindType == twoBehindType ? oneBehindType : invalidType;
        }
    }
}
