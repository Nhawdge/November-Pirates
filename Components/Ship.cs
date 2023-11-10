using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Ship
    {
        public BoatCondition BoatCondition = BoatCondition.Good;

        public int Health = 100;

        public BoatColor BoatColor;
        public BoatType BoatType;
        public SailStatus Sail;
        public Team Team;

        public int Crew;
        internal int NextPatrolPoint;
        internal Vector2 Target;
        internal List<Vector2> Route = new();
        internal float RowingPower = 100f;
    }
}
