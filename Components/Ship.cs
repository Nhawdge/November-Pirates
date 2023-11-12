using NovemberPirates.Utilities;
using System;
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
        internal List<Cannon> Cannons = new List<Cannon>();

        internal int Crew = 10;
        internal int NextPatrolPoint = 1;
        internal Vector2 Target;
        internal List<Vector2> Route = new();
        internal float RowingPower = 100f;
        internal float RotationSpeed = 100f;

        internal float MaxSpeed = 500f;
        internal float HullHealth = 100f;

        // 1 - steering only, drifts in the wind.
        // 2 - Steering and rowing
        // 3 - half-mast sailing
        // 5 - one cannon
        // 7 - two cannons
        // 10 - full sail
    }
}
