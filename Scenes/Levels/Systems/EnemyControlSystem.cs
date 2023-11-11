using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities.Maps;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyControlSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();
            var wind = singletonEntity.Get<Wind>();

            var enemyQuery = new QueryDescription().WithAll<Sprite, Ship, Npc>();
            var patrolQuery = new QueryDescription().WithAll<PatrolPoint>();

            world.Query(in enemyQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var ship = entity.Get<Ship>();
                var shouldMakeNewFire = Random.Shared.Next(0, 100) < 5;

                if (ship.BoatCondition == Utilities.BoatCondition.Broken && shouldMakeNewFire)
                {
                    EffectsBuilder.CreateFire(world, sprite.Position + new Vector2(Random.Shared.Next(-30, 30), Random.Shared.Next(-50, 30)));
                    if (ship.Crew > 0 && Random.Shared.Next(0, 100) < 5)
                    {
                        ship.Crew -= 1;
                        ship.HullHealth += 1;
                        PickupBuilder.CreateCrewMember(world, sprite.Position);
                    }
                }
                if (ship.BoatCondition == Utilities.BoatCondition.Empty)
                {
                    sprite.Position += wind.WindDirection * Raylib.GetFrameTime() * wind.WindStrength * .1f;
                }
                if (ship.HullHealth <= -100)
                {
                    world.Destroy(entity);
                }

                ship.NextPatrolPoint = 1;
                ship.Target = Vector2.Zero;

                var nextPoint = Vector2.Zero;
                world.Query(in patrolQuery, (patrolEntity) =>
                {
                    var point = patrolEntity.Get<PatrolPoint>();

                    var pointPixel = point.Position.ToPixels();
                    var distance = (sprite.Position - point.Position).Length();
                    var distanceRegular = sprite.Position.DistanceTo(point.Position);
                    if (point.Order == ship.NextPatrolPoint && distance < 50)
                    {
                        ship.NextPatrolPoint += 1;
                    }

                    if (point.Order == ship.NextPatrolPoint)
                    {
                        nextPoint = point.Position;
                    }
                });

                if (ship.Route.Count == 0)
                {
                    ship.Route = new List<Vector2>();

                    var shipTile = singleton.Map.GetTileFromPosition(sprite.Position);

                    MapPath pathToTarget = null;
                    var targetTile = singleton.Map.GetTileFromPosition(nextPoint);

                    var last = new MapPath(
                        shipTile.Coordinates,
                        shipTile.Coordinates.DistanceTo(nextPoint),
                        shipTile.Coordinates.DistanceTo(shipTile.Coordinates),
                        shipTile.MovementCost);

                    var openTiles = new List<MapPath>();
                    var closedTiles = new List<MapPath>();

                    var neighbors = singleton.Map.GetTileNeighborsForTile(shipTile).Select(neighbor =>
                        new MapPath(
                            neighbor.Coordinates,
                            neighbor.Coordinates.DistanceTo(nextPoint),
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
                        neighbors = singleton.Map.GetTileNeighborsForCoords(openTile.Coords)
                        .Select(neighbor =>
                            new MapPath(
                                neighbor.Coordinates,
                                neighbor.Coordinates.DistanceTo(targetTile.Coordinates),
                                neighbor.Coordinates.DistanceTo(shipTile.Coordinates),
                                neighbor.MovementCost,
                                last)
                            ).Where(path => !closedTiles.Contains(path));

                        openTiles.AddRange(neighbors);
                        openTiles.Remove(openTile);
                        closedTiles.Add(openTile);
                        Console.WriteLine($"Open Count: {openTiles.Count()}\t Closed Count: {closedTiles.Count()}\t{openTile.DistanceFrom} => {openTile.DistanceTo}");
                    }

                    while (pathToTarget.Parent is not null)
                    {
                        ship.Route.Insert(0, pathToTarget.Coords.ToPixels());
                        pathToTarget = pathToTarget.Parent;
                    }
                }

                var sailTarget = ship.Route.First();
                if (sprite.Position.DistanceTo(sailTarget) < 20)
                {
                    ship.Route.RemoveAt(0);
                }

                Raylib.DrawLine((int)sailTarget.X, (int)sailTarget.Y, (int)sprite.Position.X, (int)sprite.Position.Y, Raylib.RED);

                var targetDirection = Vector2.Normalize(sprite.Position - sailTarget);

                var rotationInDegrees = Math.Atan2(targetDirection.Y, targetDirection.X) * (180 / Math.PI);
                sprite.Rotation = (float)rotationInDegrees;

                ship.Sail = SailStatus.Full;
                //var movement = new Vector2(15, 0);
                //movement = RayMath.Vector2Rotate(movement, sprite.RotationAsRadians);

                //if (ship.Sail == SailStatus.Rowing)
                //    movement = movement * ship.RowingPower;

                ////if (ship.Sail == SailStatus.Half)
                ////    movement = movement * ((windStrength / 2) + player.RowingPower);

                ////if (ship.Sail == SailStatus.Full)
                ////    movement = movement * (windStrength + player.RowingPower);

                //movement *= Raylib.GetFrameTime();
                //sprite.Position += movement;
            });
        }
    }
}
