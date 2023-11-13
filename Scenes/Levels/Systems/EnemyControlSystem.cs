using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
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

                if (ship.BoatCondition == BoatCondition.Broken && shouldMakeNewFire)
                {
                    EffectsBuilder.CreateFire(world, sprite.Position + new Vector2(Random.Shared.Next(-30, 30), Random.Shared.Next(-50, 30)));
                    if (ship.Crew > 0 && Random.Shared.Next(0, 100) < 5)
                    {
                        ship.Crew -= 1;
                        ship.HullHealth += 1;
                        var sound = world.Create<AudioEvent>();
                        sound.Set(new AudioEvent() { Key = AudioKey.CrewHitWater });
                        PickupBuilder.CreateCrewMember(world, sprite.Position);
                    }
                }
                if (ship.BoatCondition == BoatCondition.Empty)
                {
                    sprite.Position += wind.WindDirection * Raylib.GetFrameTime() * wind.WindStrength * .1f;
                }
                if (ship.HullHealth <= -100)
                {
                    world.Destroy(entity);
                }

                ship.Target = Vector2.Zero;

                var nextPoint = Vector2.Zero;

                var maxPatrolPoint = 0;
                if (nextPoint == Vector2.Zero)
                {
                    world.Query(in patrolQuery, (patrolEntity) =>
                    {
                        var point = patrolEntity.Get<PatrolPoint>();
                        maxPatrolPoint = Math.Max(maxPatrolPoint, point.Order);
                        if (point.Order == ship.NextPatrolPoint)
                        {
                            nextPoint = point.Position;
                        }
                    });
                }

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

                        //Console.WriteLine($"Open Count: {openTiles.Count()}\t Closed Count: {closedTiles.Count()}\t{openTile.DistanceFrom} => {openTile.DistanceTo}");
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
                    ship.NextPatrolPoint += 1;
                    if (ship.NextPatrolPoint > maxPatrolPoint)
                        ship.NextPatrolPoint = 1;
                }

                if (singleton.Debug >= DebugLevel.Low)
                    Raylib.DrawLine((int)sailTarget.X, (int)sailTarget.Y, (int)sprite.Position.X, (int)sprite.Position.Y, Raylib.RED);

                var targetDirection = Vector2.Normalize(sprite.Position - sailTarget);

                var rotationInDegrees = Math.Atan2(targetDirection.Y, targetDirection.X) * (180 / Math.PI);
                if (sprite.Rotation > rotationInDegrees)
                {
                    var rotationNeeded = (float)Math.Min(sprite.Rotation - rotationInDegrees, ship.RotationSpeed * Raylib.GetFrameTime());
                    sprite.Rotation -= rotationNeeded;
                }
                else
                {
                    var rotationNeeded = (float)Math.Min(rotationInDegrees - sprite.Rotation, ship.RotationSpeed * Raylib.GetFrameTime());
                    sprite.Rotation += ship.RotationSpeed * Raylib.GetFrameTime();
                }
                ship.Sail = SailStatus.Full;
            });
        }
    }
}
