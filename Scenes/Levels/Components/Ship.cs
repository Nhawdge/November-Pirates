﻿using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Data;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Ship
    {
        public Ship(HullType hull, BoatColor color, Team team)
        {
            HullType = hull;
            BoatColor = color;
            Team = team;

            Crew = (int)ShipData.Instance.Data[$"{hull}{Stats.InitialCrew}"];
            MaxSpeed = ShipData.Instance.Data[$"{hull}{Stats.MaxSpeed}"];
            RowingPower = ShipData.Instance.Data[$"{hull}{Stats.RowingSpeed}"];
            RotationSpeed = ShipData.Instance.Data[$"{hull}{Stats.TurningSpeed}"];
            HullHealth = ShipData.Instance.Data[$"{hull}{Stats.HullHealth}"];
            SailHealth = ShipData.Instance.Data[$"{hull}{Stats.SailHealth}"];
            HalfSailSpeedModifier = ShipData.Instance.Data[$"{hull}{Stats.HalfSailSpeed}"];
            FullSailSpeedModifier = ShipData.Instance.Data[$"{hull}{Stats.FullSailSpeed}"];
            SailSpeedEasing = ShipData.Instance.Data[$"{hull}{Stats.SailSpeedEasing}"];
        }
        public BoatCondition BoatCondition = BoatCondition.Good;

        public BoatColor BoatColor;
        public HullType HullType;
        public Team Team;
        internal SailStatus Sail = SailStatus.Closed;

        internal List<Cannon> Cannons = new List<Cannon>();

        internal float WindInSail;

        internal int Crew;
        internal float RowingPower;
        internal float RotationSpeed;

        internal float MaxSpeed;
        internal float HullHealth;
        internal float SailHealth;
        internal float HalfSailSpeedModifier;
        internal float FullSailSpeedModifier;
        internal float SailSpeedEasing;
        internal float Currency;

        public Task<List<Vector2>> NavTask { get; set; }
    }
}
