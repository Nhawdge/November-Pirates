using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using QuickType.Map;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class ShipControlSystem : GameSystem
    {
        public ShipControlSystem()
        {
        }

        internal override void Update(World world)
        {
            var shipsQuery = new QueryDescription().WithAll<Ship, Sprite>();
            var tiles = new QueryDescription().WithAll<MapTile, Render>();

            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();
            Wind wind = singletonEntity.Get<Wind>();

            world.Query(shipsQuery, (entity) =>
            {
                var ship = entity.Get<Ship>();
                var sprite = entity.Get<Sprite>();

                var movement = new Vector2(0, 0);

                if (ship.Sail >= SailStatus.Rowing)
                {
                    movement = new Vector2(0, -1);
                }

                movement = RayMath.Vector2Rotate(movement, sprite.RotationAsRadians);

                var boatAngle = (sprite.RenderRotation + 360) % 360;
                var windAngle = (float)(((Math.Atan2(wind.WindDirection.Y, wind.WindDirection.X) * 180 / Math.PI) + 360 + 90) % 360);
                var angleDiff = Math.Abs((boatAngle + 360) - (windAngle + 360)) % 360;

                var windInSail = angleDiff switch
                {
                    < 15 => 1f,
                    < 30 => 0.75f,
                    < 45 => 0.5f,
                    < 60 => 0.25f,
                    < 75 => 0.1f,
                    _ => 0f,
                };

                if (windInSail == 1f && windInSail != ship.WindInSail && ship.Sail >= SailStatus.Half)
                    world.Create(new AudioEvent() { Key = AudioKey.WindInSail, Position = sprite.Position });

                ship.WindInSail = windInSail;
                //singleton.DebugText += $"Boat Angle:{boatAngle.ToString("0.0")}\nWind Angle: {windAngle.ToString("0.0")}\nAngle Diff: {angleDiff.ToString("0.0")}\nWind In Sail: {windInSail.ToString("0.0")}";
                var windStrength = wind.WindStrength * windInSail;

                //singleton.DebugText += $"\nWind: {windStrength} ";
                //singleton.DebugText += $"\nRow: {ship.RowingPower} ";

                if (ship.Sail == SailStatus.Closed)
                    movement = new Vector2(0, 0);
                else
                {
                    var inRadians = (float)((sprite.RenderRotation + 45) * (Math.PI / 180));
                    EffectsBuilder.CreateWaterTrail(world, sprite.Position, RayMath.Vector2Rotate(new Vector2(50, 0), inRadians));
                    EffectsBuilder.CreateWaterTrail(world, sprite.Position, RayMath.Vector2Rotate(new Vector2(0, 50), inRadians));
                    EffectsBuilder.CreateWaterTrail(world, sprite.Position, RayMath.Vector2Rotate(new Vector2(40, 0), inRadians));
                    EffectsBuilder.CreateWaterTrail(world, sprite.Position, RayMath.Vector2Rotate(new Vector2(0, 40), inRadians));
                    EffectsBuilder.CreateWaterTrail(world, sprite.Position, RayMath.Vector2Rotate(new Vector2(20, 20), inRadians));
                }

                if (ship.Sail == SailStatus.Rowing)
                    movement *= ship.RowingPower;

                if (ship.Sail == SailStatus.Half)
                    movement *= ((windStrength / 2) + ship.RowingPower);

                if (ship.Sail == SailStatus.Full)
                    movement *= (windStrength + ship.RowingPower);

                movement *= Raylib.GetFrameTime();

                var adjustedPosition = sprite.Position
                    with
                {
                    X = sprite.Position.X,
                    Y = sprite.Position.Y,
                };

                if (singleton.Debug >= DebugLevel.Medium)
                    Raylib.DrawCircle((int)adjustedPosition.X, (int)adjustedPosition.Y, 50f, Raylib.ORANGE);

                var newPosition = adjustedPosition + movement;

                singleton.DebugText += $"\nMovement: {movement.ToString("0.0")}\n{movement.Length().ToString("0.0")} \nNew Position: {newPosition.ToString("0.0")}\n";

                var query = world.Query(in tiles);

                Render collidingTile = null;
                foreach (ref var chunk in query.GetChunkIterator())
                {
                    foreach (var tile in chunk)
                    {
                        var tileSprite = chunk.Get<Render>(tile);

                        if (Raylib.CheckCollisionCircleRec(newPosition, 50f, tileSprite.Destination))
                        {
                            if (collidingTile == null)
                            {
                                collidingTile = tileSprite;
                            }
                            else if (tileSprite.Collision > collidingTile?.Collision)
                            {
                                collidingTile = tileSprite;
                            }
                        }
                    }
                }
                if (collidingTile != null)
                {
                    if (collidingTile.Collision == CollisionType.Slow)
                    {
                        sprite.Position += movement / 2/* * Raylib.GetFrameTime()*/;
                        //Console.WriteLine("I am on a slow tile");
                    }
                    else if (collidingTile.Collision == CollisionType.None)
                    {
                        sprite.Position += movement /* * Raylib.GetFrameTime()*/;
                        //Console.WriteLine("Smooth sailing");
                    }
                    else if (collidingTile.Collision == CollisionType.Solid)
                    {
                        //Console.WriteLine($"Collision {collidingTile.Collision}");
                    }
                }

                ship.Cannons.ForEach(cannon =>
                {
                    cannon.ReloadElapsed = Math.Min(cannon.ReloadTime, cannon.ReloadElapsed + Raylib.GetFrameTime());
                });
            });
        }
    }
}
