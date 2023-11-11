using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Ship
    {
        public BoatCondition BoatCondition = BoatCondition.Good;

        public BoatColor BoatColor;
        public BoatType BoatType;
        public Team Team;
        internal SailStatus Sail = SailStatus.Closed;

        internal int Crew = 10;
        internal int NextPatrolPoint;
        internal Vector2 Target;
        internal List<Vector2> Route = new();
        internal float RowingPower = 100f;
        internal float RotationSpeed = 100f;

        internal float MaxSpeed = 500f;
        internal float HullHealth = 100f;
    }
}
