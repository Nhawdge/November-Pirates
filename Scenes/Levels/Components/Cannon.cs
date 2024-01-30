using NovemberPirates.Utilities.Data;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Cannon
    {
        internal CannonType CannonType;

        internal Vector2 Position;
        internal BoatSide Placement;
        internal int Row;

        internal float ReloadElapsed = 0f;
        internal float ReloadTime;
        internal float ReloadRate;
        internal float HullDamage;
        internal float SailDamage;
        internal float CrewKillChance;
        internal float CrewKillChainLimit;
        internal float Spread;
        internal float BallSpeed;
        internal float BallDuration;
        internal float ShotPer;
    }

    internal enum BoatSide
    {
        Port,
        Starboard
    }
}
