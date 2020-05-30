namespace core
{
    public class SwapData
    {
        private readonly Coordinate position1;
        private readonly Coordinate position2;

        SwapData(Coordinate position1, Coordinate position2)
        {
            this.position1 = position1;
            this.position2 = position2;
        }

        public Coordinate Position1
        {
            get => position1;
        }

        public Coordinate Position2
        {
            get => position2;
        }
    }
} // namespace core