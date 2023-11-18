using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Cannonball
    {
        internal Vector2 Motion;
        internal Team FiredBy;
        internal float Duration = 0.75f;
        internal float Elapsed = 0f;
        internal Cannon FiredByCannon;
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
