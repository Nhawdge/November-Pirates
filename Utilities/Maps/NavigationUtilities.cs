using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities.Data;
using System.Numerics;

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

        internal static void AddTradeRoutes(World world)
        {
            var allPorts = new List<Port>();

            world.Query(in new QueryDescription().WithAll<Port>(), (entity) =>
            {
                allPorts.Add(entity.Get<Port>());
            });


            for (var i = 0; i < 1; i++)
            {
                var routePorts = allPorts.OrderBy(x => Random.Shared.Next()).Take(3).ToList();
                var routes = new List<Route>();
                var key = $"Route-{i}";
                if (RouteDataStore.Instance.Routes.ContainsKey(key))
                {
                    continue;
                }
                var index = 0;
                var calcTasks = new List<Task>();
                foreach (var port in routePorts)
                {
                    calcTasks.Add(new Task(() =>
                    {
                        index++;
                        var secondPort = routePorts.ElementAtOrDefault(index);
                        if (secondPort is null)
                        {
                            index = 0;
                            secondPort = routePorts.ElementAt(index);
                        }
                        var route = new Route();

                        route.FromPort = port;
                        route.ToPort = secondPort;
                        Console.WriteLine($"{key} - {route.FromPort.ShortId()} {route.ToPort.ShortId()}");
                        route.RoutePoints = CalculateRoute(world, route.FromPort.Position, route.ToPort.Position).ToList();
                        routes.Add(route);
                    }));
                }
                foreach (var task in calcTasks)
                {
                    task.Start();
                }
                Task.WaitAll(calcTasks.ToArray());

                RouteDataStore.Instance.Routes.Add(key, routes);
            }
        }

        internal static IEnumerable<Vector2> CalculateRouteFromShip(World world, Arch.Core.Entity shipEntity)
        {
            var sprite = shipEntity.Get<Sprite>();
            var ship = shipEntity.Get<Ship>();
            return CalculateRoute(world, sprite.Position, ship.Goal);
        }

        internal static IEnumerable<Vector2> CalculateRoute(World world, Vector2 from, Vector2 to)
        {
            var singleton = world.QueryFirst<Singleton>().Get<Singleton>();

            var shipTile = singleton.Map.GetTileFromPosition(from);

            MapPath pathToTarget = null;
            var targetTile = singleton.Map.GetTileFromPosition(to);

            var last = new MapPath(
                    shipTile.Coordinates,
                    shipTile.Coordinates.DistanceTo(to),
                    shipTile.Coordinates.DistanceTo(shipTile.Coordinates),
                    shipTile.MovementCost);

            var openTiles = new List<MapPath>();
            var closedTiles = new List<Vector2>();

            var neighbors = singleton.Map.GetTileNeighborsForTile(shipTile).Select(neighbor =>
                    new MapPath(
                        neighbor.Coordinates,
                        neighbor.Coordinates.DistanceTo(to),
                        neighbor.Coordinates.DistanceTo(shipTile.Coordinates),
                        neighbor.MovementCost,
                        last)
                    );
            openTiles.AddRange(neighbors);

            while (pathToTarget is null)
            {
                var openTile = openTiles.OrderBy(tile => tile.TotalCost).ThenBy(tile => tile.DistanceTo).First();
                if (openTile.Coords == targetTile.Coordinates)
                {
                    pathToTarget = openTile;
                    break;
                }
                closedTiles.Add(openTile.Coords);

                neighbors = singleton.Map.GetTileNeighborsForCoords(openTile.Coords)
                    .Where(x => !closedTiles.Contains(x.Coordinates))
                    .Select(neighbor =>
                        new MapPath(
                            neighbor.Coordinates,
                            neighbor.Coordinates.DistanceTo(targetTile.Coordinates),
                            neighbor.Coordinates.DistanceTo(shipTile.Coordinates),
                            neighbor.MovementCost,
                            openTile)
                        );

                openTiles.AddRange(neighbors);
                openTiles.RemoveAll(x => x.Coords == openTile.Coords);
            }
            var route = new List<Vector2>();
            while (pathToTarget.Parent is not null)
            {
                route.Insert(0, pathToTarget.Coords.ToPixels());
                pathToTarget = pathToTarget.Parent;
            }
            return route;
        }
    }
}