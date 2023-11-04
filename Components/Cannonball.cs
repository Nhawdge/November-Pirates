using System.Numerics;

namespace NovemberPirates.Components
{
    internal class Cannonball
    {
        internal Vector2 Motion;
        internal Team FiredBy;
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
