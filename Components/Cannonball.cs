using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Cannonball
    {
        internal Vector2 Motion;
        internal Team FiredBy;
        internal float Duration = 8f;
        internal float Elapsed = 0f;
    }
    internal enum Team
    {
        None,
        Player,
        Red,
        Green,
        Blue,
        Yellow
    }
}
