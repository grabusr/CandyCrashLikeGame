namespace QuickTurnStudio.CandyCrashLike.Core
{
    public class SwapData
    {
        private readonly Coordinate first;
        private readonly Coordinate second;

        public SwapData(Coordinate position1, Coordinate position2)
        {
            if (position1.Row != position2.Row)
            {
                first = position1.Row < position2.Row ? position1 : position2;
                second = position1.Row < position2.Row ? position2 : position1;
            }
            else
            {
                first = position1.Column < position2.Column ? position1 : position2;
                second = position1.Column < position2.Column ? position2 : position1;
            }
        }

        public Coordinate First
        {
            get => first;
        }

        public Coordinate Second
        {
            get => second;
        }
    }
}