using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public struct Coordinate
    {
        private readonly int row;
        private readonly int column;

        public Coordinate(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int Row
        {
            get => row;
        }

        public int Column
        {
            get => column;
        }

        public bool Equals(Coordinate coordinate)
        {
            return row == coordinate.row && column == coordinate.column;
        }

        public string ToString()
        {
            return "(" + row + ", " + column + ")";
        }
}

} // namespace core
