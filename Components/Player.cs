using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Player
    {
        internal Vector2 Position;
        internal float Speed = 500f;
        internal float RotationSpeed = 100f;
        internal SailStatus Sail = SailStatus.Closed;

        internal float RowingPower = 25f;
    }

    public enum SailStatus
    {
        Closed,
        Rowing,
        Half,
        Full,
    }
}
