using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
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
            var sprites = new QueryDescription().WithAll<Player, Sprite>();
            var tiles = new QueryDescription().WithAll<MapTile, Render>();

            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();
            Wind wind = singletonEntity.Get<Wind>();

            world.Query(sprites, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var player = entity.Get<Player>();

                var movement = new Vector2(0, 0);
                var boatChanged = false;

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    player.Sail = (SailStatus)Math.Min(Enum.GetValues<SailStatus>().Length - 1, (int)player.Sail + 1);
                    boatChanged = true;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    player.Sail = (SailStatus)Math.Max(0, (int)player.Sail - 1);
                    boatChanged = true;
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                {
                    sprite.Rotation -= player.RotationSpeed * Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    sprite.Rotation += player.RotationSpeed * Raylib.GetFrameTime();
                }
                if (player.Sail >= SailStatus.Rowing)
                {
                    movement = new Vector2(0, -1);
                }

                if (boatChanged)
                    sprite.Texture = (ShipSpriteBuilder.GenerateBoat(new BoatOptions(BoatType.HullLarge, BoatColor.Dead, player.Sail, player.BoatCondition))).Texture;

                movement = RayMath.Vector2Rotate(movement, sprite.RotationAsRadians);

                var boatAngle = (sprite.Rotation + 360) % 360;
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
                //singleton.DebugText += $"Boat Angle:{boatAngle.ToString("0.0")}\nWind Angle: {windAngle.ToString("0.0")}\nAngle Diff: {angleDiff.ToString("0.0")}\nWind In Sail: {windInSail.ToString("0.0")}";
                var windStrength = wind.WindStrength * windInSail;

                singleton.DebugText += $"\nWind:{windStrength} ";
                singleton.DebugText += $"\nRow: {player.RowingPower} ";

                if (player.Sail == SailStatus.Closed)
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
                if (player.Sail == SailStatus.Rowing)
                    movement = movement * player.RowingPower;

                if (player.Sail == SailStatus.Half)
                    movement = movement * ((windStrength / 2) + player.RowingPower);

                if (player.Sail == SailStatus.Full)
                    movement = movement * (windStrength + player.RowingPower);

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

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
                {
                    // fire cannon Port
                    CannonballBuilder.Create(world, sprite.Position, sprite.RenderRotation + 180);
                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = sprite.Position,Key = AudioKey.CannonFire } );
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_E) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
                {
                    // fire cannon Starboard
                    CannonballBuilder.Create(world, sprite.Position, sprite.RenderRotation);
                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = sprite.Position, Key = AudioKey.CannonFire });
                }
            });
        }
    }
}
