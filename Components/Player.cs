using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Player
    {
        internal Vector2 Position;
        internal float MaxSpeed = 500f; 
        internal float RotationSpeed = 100f;
        internal SailStatus Sail = SailStatus.Closed;

        internal float RowingPower = 100f;
    }

    public enum SailStatus
    {
        Closed,
        Rowing,
        Half,
        Full,
    }
}
