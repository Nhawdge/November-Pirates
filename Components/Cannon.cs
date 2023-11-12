using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Cannon
    {
        internal BoatSide Placement;

        internal float ReloadTime =0.5f;
        internal float ReloadElapsed = 0f;
        internal Vector2 Position;
        internal int Row;
    }

    internal enum BoatSide
    {
        Port,
        Starboard
    }
}
