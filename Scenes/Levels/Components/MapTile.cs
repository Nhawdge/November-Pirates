using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class MapTile
    {
        public MapTile(Vector2 coords, long gridSize, float cost = 1)
        {
            Coordinates = coords;
            MovementCost = cost;
            Position = new Vector2(coords.X * gridSize, coords.Y * gridSize);
        }
        internal Vector2 Coordinates;
        internal float MovementCost;
        internal Vector2 Position;
    }
}
