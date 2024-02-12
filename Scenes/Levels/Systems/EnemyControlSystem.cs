using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Data;
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
                var npc = entity.Get<Npc>();

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

                //if (ship.BoatCondition == BoatCondition.Empty)
                //{
                //    sprite.Position += wind.WindDirection * Raylib.GetFrameTime() * wind.WindStrength * .1f;
                //}

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

                if (npc.TargetPosition.DistanceTo(sprite.Position) < 200)
                {
                    npc.TimeSinceLastGoalChange += 100;
                }

                if (singleton.Debug >= DebugLevel.Low)
                    Raylib.DrawLine((int)npc.TargetPosition.X, (int)npc.TargetPosition.Y, (int)sprite.Position.X, (int)sprite.Position.Y, Raylib.RED);

                if (ship.CanDo(ShipAbilities.Steering))
                {
                    var targetDirection = Vector2.Normalize(sprite.Position - npc.TargetPosition);
                    var targetDirectionInDegrees = (float)(Math.Atan2(targetDirection.Y, targetDirection.X) * (180 / Math.PI));

                    var rotationInDegreesToTarget = (float)(Math.Atan2(targetDirection.Y, targetDirection.X) * (180 / Math.PI)) + npc.TargetOffsetInDegrees;

                    // whats in front of me, should I turn

                    var distanceToScan = ship.Sail switch
                    {
                        SailStatus.Half => 200,
                        SailStatus.Full => 300,
                        _ => 100,
                    };

                    var frontLeft = sprite.Position + RayMath.Vector2Rotate(new Vector2(0, distanceToScan), (sprite.Rotation + 45).ToRadians());
                    var posInFront = sprite.Position + RayMath.Vector2Rotate(new Vector2(0, distanceToScan), (sprite.Rotation + 90).ToRadians());
                    var frontRight = sprite.Position + RayMath.Vector2Rotate(new Vector2(0, distanceToScan), (sprite.Rotation + 135).ToRadians());

                    if (singleton.Debug > 0)
                    {
                        Raylib.DrawCircle((int)frontLeft.X, (int)frontLeft.Y, 10, Raylib.RED);
                        Raylib.DrawCircle((int)posInFront.X, (int)posInFront.Y, 10, Raylib.GREEN);
                        Raylib.DrawCircle((int)frontRight.X, (int)frontRight.Y, 10, Raylib.DARKBLUE);
                    }

                    var leftGood = false;
                    var centerGood = false;
                    var rightGood = false;

                    var tileInFront = singleton.Map.GetTileFromPosition(posInFront);
                    if (tileInFront?.MovementCost == 1)
                    {
                        centerGood = true;
                    }
                    var tileFrontRight = singleton.Map.GetTileFromPosition(frontRight);
                    if (tileFrontRight?.MovementCost == 1)
                    {
                        rightGood = true;
                    }
                    var tileFrontLeft = singleton.Map.GetTileFromPosition(frontLeft);

                    if (tileFrontLeft?.MovementCost == 1)
                    {
                        leftGood = true;
                    }

                    if (!centerGood && !leftGood)
                    {
                        var moveBy = tileFrontLeft?.MovementCost > 2 ? 3f : 1f;

                        if (ship.Sail == SailStatus.Rowing)
                            moveBy = 1f;

                        if (Math.Abs(npc.TargetOffsetInDegrees) > 90 && Vector2.Dot(sprite.RotationAsVector2, targetDirection) < 0)
                            npc.TargetOffsetInDegrees += 200;
                        else
                            npc.TargetOffsetInDegrees += moveBy;
                    }
                    else if (!centerGood && !rightGood)
                    {
                        var moveBy = tileFrontRight?.MovementCost > 2 ? 3f : 1f;

                        if (ship.Sail == SailStatus.Rowing)
                            moveBy = 1f;

                        if (Math.Abs(npc.TargetOffsetInDegrees) > 90 && Vector2.Dot(sprite.RotationAsVector2, targetDirection) < 0)
                            npc.TargetOffsetInDegrees -= 200;
                        else
                            npc.TargetOffsetInDegrees -= moveBy;
                    }
                    else if (!leftGood)
                        npc.TargetOffsetInDegrees += 2;
                    else if (!rightGood)
                        npc.TargetOffsetInDegrees -= 2;
                    else
                    {
                        if (rotationInDegreesToTarget > targetDirectionInDegrees)
                            npc.TargetOffsetInDegrees -= 1;
                        else if (rotationInDegreesToTarget < targetDirectionInDegrees)
                            npc.TargetOffsetInDegrees += 1;
                    }

                    if (sprite.Rotation != rotationInDegreesToTarget)
                    {
                        var rotationNeeded = (float)Math.Min(sprite.Rotation - rotationInDegreesToTarget, ship.RotationSpeed * Raylib.GetFrameTime());
                        if (Math.Abs(rotationInDegreesToTarget) > 1)
                        {
                            sprite.Rotation -= rotationNeeded;
                        }
                    }
                    else
                    {
                        var rotationNeeded = (float)Math.Min(rotationInDegreesToTarget - sprite.Rotation, ship.RotationSpeed * Raylib.GetFrameTime());
                        if (Math.Abs(rotationInDegreesToTarget) > 1)
                        {
                            sprite.Rotation += rotationNeeded;
                        }
                        sprite.Rotation += ship.RotationSpeed * Raylib.GetFrameTime();
                    }
                }


                if (singleton.Debug > DebugLevel.None)
                {
                    //Raylib.DrawText($"Route Size:{ship.Route?.Count} \nTarget:{ship.Route?.LastOrDefault()}", sprite.Position.X, sprite.Position.Y, 12, Raylib.RED);
                }

                ship.Sail = SailStatus.Closed;
                sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;

                if (ship.CanDo(ShipAbilities.FullSail) && sprite.Position.DistanceTo(npc.TargetPosition) > 1000)
                {
                    ship.Sail = SailStatus.Full;
                    sprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                }
                else if (ship.CanDo(ShipAbilities.HalfSail) && sprite.Position.DistanceTo(npc.TargetPosition) > 400)
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
