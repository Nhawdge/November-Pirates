using NovemberPirates.Utilities;

namespace NovemberPirates.Components
{
    internal class Ship
    {
        public BoatCondition BoatCondition = BoatCondition.Good;

        public int Health = 100;

        public BoatColor BoatColor { get; internal set; }
        public BoatType BoatType { get; internal set; }
        public SailStatus Sail { get; internal set; }
    }
}
