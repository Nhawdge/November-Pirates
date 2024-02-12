using Arch.Core;
using NovemberPirates.Scenes.Levels.Components;
using System.Numerics;

namespace NovemberPirates.Utilities.Maps
{
    internal class Map
    {
        internal List<MapTile> Tiles = new();
        internal int TileSize = 64;
        internal bool CheckInProgress = false;

        internal MapTile GetTileFromPosition(Vector2 position)
        {
            var coords = new Vector2((int)(position.X / TileSize), (int)(position.Y / TileSize));
            return Tiles.FirstOrDefault(x => x.Coordinates == coords);
        }

        internal List<MapTile> GetTileNeighborsForTile(MapTile tile)
        {
            var coords = new Vector2(tile.Coordinates.X, tile.Coordinates.Y);
            return GetTileNeighborsForCoords(coords);
        }

        internal List<MapTile> GetTileNeighborsForCoords(Vector2 coords)
        {
            var neighbors = new List<MapTile>();

            var left = Tiles.FirstOrDefault(x => x.Coordinates == coords + new Vector2(-1, 0));
            var right = Tiles.FirstOrDefault(x => x.Coordinates == coords + new Vector2(1, 0));
            var up = Tiles.FirstOrDefault(x => x.Coordinates == coords + new Vector2(0, -1));
            var down = Tiles.FirstOrDefault(x => x.Coordinates == coords + new Vector2(0, 1));

            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);
            if (up != null) neighbors.Add(up);
            if (down != null) neighbors.Add(down);

            return neighbors;
        }

        internal List<MapTile> GetTilesInSquareRange(Vector2 startPos, int range)
        {
            var startPosCoords = new Vector2((int)(startPos.X / TileSize), (int)(startPos.Y / TileSize));
            var tiles = new List<MapTile>();

            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    var coords = new Vector2(startPosCoords.X + x, startPosCoords.Y + y);
                    var tile = Tiles.FirstOrDefault(t => t.Coordinates == coords);
                    if (tile != null)
                    {
                        tiles.Add(tile);
                    }
                }
            }

            return tiles;
        }
    }
}
