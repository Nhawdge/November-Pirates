using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;

namespace NovemberPirates.Utilities.Maps
{
    internal static class NavigationUtilities
    {
        internal static void BuildMap(World world)
        {
            var singleton = world.QueryFirst<Singleton>().Get<Singleton>();

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
                        tile = new MapTile
                        {
                            Coordinates = maptile.Coordinates
                        };
                        map.Tiles.Add(tile);
                    }
                    tile.MovementCost = Math.Max(tile.MovementCost, maptile.MovementCost);
                });
                singleton.Map = map;
            }
        }
    }

}