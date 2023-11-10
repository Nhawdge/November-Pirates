using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities.Maps;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class NavigationSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            if (singleton.Map is null)
            {
                var map = new Map();

                var tilesQuery = new QueryDescription().WithAll<MapTile>();

                world.Query(in tilesQuery, (entity) =>
                {
                    var maptile = entity.Get<MapTile>();
                    var tile = map.Tiles.FirstOrDefault(x => x.Coordinates == maptile.Coordinates);

                    if (tile is null)
                    {
                        tile = new MapTile { Coordinates = maptile.Coordinates };
                        map.Tiles.Add(tile);
                    }
                    tile.MovementCost = Math.Max(tile.MovementCost, maptile.MovementCost);
                });
                singleton.Map = map;
            }
            if (singleton.Debug >= DebugLevel.Medium)
                foreach (var tile in singleton.Map.Tiles)
                {
                    var color = tile.MovementCost switch
                    {
                        1 => Raylib.BLUE,
                        2 => Raylib.YELLOW,
                        > 10 => Raylib.BLACK,
                        _ => Raylib.WHITE
                    };
                    Raylib.DrawText($"{tile.Coordinates.X}, {tile.Coordinates.Y}\n{tile.MovementCost}", tile.Coordinates.X * 64, tile.Coordinates.Y * 64, 8, color);
                }

        }
    }
}
