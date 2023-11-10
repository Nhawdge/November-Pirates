using NovemberPirates.Components;
using System.Numerics;

namespace NovemberPirates.Utilities.Maps
{
    public record MapPath(Vector2 Coords, float DistanceTo, float DistanceFrom, float Cost, MapPath Parent = null)
    {
        public float TotalCost => Cost + (Parent?.TotalCost ?? 0);
    }
}
