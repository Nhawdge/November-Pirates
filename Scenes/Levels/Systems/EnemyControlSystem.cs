﻿using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Data;
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
                        sound.Set(new AudioEvent() { Key = AudioKey.CrewHitWater, Position = sprite.Position });
                        PickupBuilder.CreateCrewMember(world, sprite.Position);
                    }
                }

                if (ship.BoatCondition == BoatCondition.Empty)
                {
                    sprite.Position += wind.WindDirection * Raylib.GetFrameTime() * wind.WindStrength * .1f;
                }
                if (ship.HullHealth < 0)
                {
                    ship.Crew = 0;
                    var percent = (100 + ship.HullHealth) / 100;
                    sprite.Color.a = (byte)(255 * percent);
                    ship.HullHealth -= Raylib.GetFrameTime();
                }
                if (ship.HullHealth <= -100)
                {
                    world.Destroy(entity);
                    return;
                }

                var maxPatrolPoint = 0;

                var npc = entity.Get<Npc>();
                if (npc.Purpose == Purpose.Patrol)
                {
                    if (ship.Goal == Vector2.Zero)
                    {
                        world.Query(in patrolQuery, (patrolEntity) =>
                        {
                            var point = patrolEntity.Get<PatrolPoint>();
                            maxPatrolPoint = Math.Max(maxPatrolPoint, point.Order);
                            if (point.Order == ship.NextPatrolPoint)
                            {
                                ship.Goal = point.Position;
                            }
                        });
                    }
                }
                if (npc.Purpose == Purpose.Trade)
                { 
                    if (ship.Route.Count == 0) 
                    {
                        ship.Route = ship.SailingRoute.First().RoutePoints;
                        ship.SailingRoute.Add(ship.SailingRoute.First());
                        ship.SailingRoute.RemoveAt(0);
                    }
                }
                else if (npc.Purpose == Purpose.Patrol)
                {
                    if ((ship.Route.Count < 10))

                    {
                        if (ship.NavTask == null)
                            ship.NavTask = new Task<List<Vector2>>(() => NavigationUtilities.CalculateRouteFromShip(world, entity).ToList());

                        if (ship.NavTask.IsCompleted)
                        {
                            ship.Route = ship.NavTask.Result;
                            ship.NavTask = null;
                        }
                        else if (ship.NavTask.Status == TaskStatus.Created)
                        {
                            ship.NavTask.Start();
                        }
                    }
                }
                var sailTargetVec = ship.Route?.FirstOrDefault();
                if (sailTargetVec is not null)
                {
                    ship.Target = sailTargetVec.Value;
                    if (sprite.Position.DistanceTo(ship.Target) < 300)
                    {
                        if (ship.Route?.Count > 0)
                            ship.Route?.RemoveAt(0);

                        if (ship.Route?.Count == 0 && npc.Purpose == Purpose.Patrol)
                        {
                            ship.NextPatrolPoint += 1;
                            if (ship.NextPatrolPoint > maxPatrolPoint)
                                ship.NextPatrolPoint = 1;
                        }
                        if (ship.Route?.Count == 0 && npc.Purpose == Purpose.Trade)
                        {
                            ship.Goal = Vector2.Zero;
                            var availableMoney = 10;
                            //ship.TargetPort.Currency -= availableMoney;
                            ship.Currency += availableMoney;
                        }
                    }
                    if (singleton.Debug >= DebugLevel.Low)
                        Raylib.DrawLine((int)ship.Target.X, (int)ship.Target.Y, (int)sprite.Position.X, (int)sprite.Position.Y, Raylib.RED);

                    if (ship.Target != Vector2.Zero)
                    {
                        if (ship.CanDo(ShipAbilities.Steering))
                        {
                            var targetDirection = Vector2.Normalize(sprite.Position - ship.Target);

                            var rotationInDegrees = Math.Atan2(targetDirection.Y, targetDirection.X) * (180 / Math.PI);
                            if (sprite.Rotation != rotationInDegrees)
                            {
                                var rotationNeeded = (float)Math.Min(sprite.Rotation - rotationInDegrees, ship.RotationSpeed * Raylib.GetFrameTime());
                                if (Math.Abs(rotationInDegrees) > 1)
                                {
                                    sprite.Rotation -= rotationNeeded;
                                }
                            }
                            else
                            {
                                var rotationNeeded = (float)Math.Min(rotationInDegrees - sprite.Rotation, ship.RotationSpeed * Raylib.GetFrameTime());
                                if (Math.Abs(rotationInDegrees) > 1)
                                {
                                    sprite.Rotation += rotationNeeded;
                                }
                                sprite.Rotation += ship.RotationSpeed * Raylib.GetFrameTime();
                            }
                        }
                    }
                }

                //Console.WriteLine($"Target {ship.Target}");

                if (singleton.Debug > DebugLevel.None)
                {
                    Raylib.DrawText($"Route Size:{ship.Route?.Count} \nTarget:{ship.Route?.LastOrDefault()}", sprite.Position.X, sprite.Position.Y, 12, Raylib.RED);
                }
                if (ship.Target == Vector2.Zero)
                {
                    ship.Sail = SailStatus.Closed;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
                else if (ship.CanDo(ShipAbilities.FullSail))
                {
                    ship.Sail = SailStatus.Full;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
                else if (ship.CanDo(ShipAbilities.HalfSail))
                {
                    ship.Sail = SailStatus.Half;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
                else if (ship.CanDo(ShipAbilities.Rowing))
                {
                    ship.Sail = SailStatus.Rowing;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
                else
                {
                    ship.Sail = SailStatus.Closed;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
            });
        }
    }
}
