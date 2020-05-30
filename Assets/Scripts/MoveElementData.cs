namespace core
{
    public class MoveElementData
    {
        private readonly Coordinate startField;
        private readonly Coordinate endField;

        public MoveElementData(Coordinate start, Coordinate destination)
        {
            this.startField = start;
            this.endField = destination;
        }

        public Coordinate Start
        {
            get => startField;
        }

        public Coordinate Destination
        {
            get => endField;
        }
    }
} // namespace core