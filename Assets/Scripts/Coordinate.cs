namespace QuickTurnStudio.CandyCrashLike.Core
{
    public struct Coordinate
    {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }        

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            var coordinate = (Coordinate)obj;
            return Row == coordinate.Row && Column == coordinate.Column;
        }

        public override string ToString()
        {
            return "(" + Row + ", " + Column + ")";
        }
    }
}
