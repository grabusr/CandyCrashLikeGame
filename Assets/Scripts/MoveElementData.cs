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

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            var moveElementData = (MoveElementData)obj;

            return Start.Equals(moveElementData.Start)
                && Destination.Equals(moveElementData.Destination);
        }
    }
} // namespace core