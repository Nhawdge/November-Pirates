using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class BoatMovementSystem : GameSystem
    {
        public BoatMovementSystem()
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

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    player.Sail = (SailStatus)Math.Min(Enum.GetValues<SailStatus>().Length - 1, (int)player.Sail + 1);
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    player.Sail = (SailStatus)Math.Max(0, (int)player.Sail - 1);
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                {
                    sprite.Rotation -= player.RotationSpeed * Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    sprite.Rotation += player.RotationSpeed * Raylib.GetFrameTime();
                }
                if (player.Sail == SailStatus.Rowing)
                {
                    movement = new Vector2(0, -1);
                }

                movement = RayMath.Vector2Rotate(movement, sprite.RotationAsRadians);

                var boatAngle = (sprite.Rotation + 360) % 360;
                var windAngle = (float)(((Math.Atan2(wind.WindDirection.Y, wind.WindDirection.X) * 180 / Math.PI) + 360 + 90) % 360);
                var angleDiff = Math.Abs(boatAngle - windAngle);
                var forceApplied = angleDiff / 90;
                var calculatedDiff = Math.Abs((float)Math.Cos(angleDiff));
                var dotp = Vector2.Dot(wind.WindDirection, movement);

                singleton.DebugText += $"\nBoat Angle:{boatAngle.ToString("0.0")}\nWind Angle: {windAngle.ToString("0.0")}\nAngle Diff: {angleDiff.ToString("0.0")}\nForce: {forceApplied.ToString("0.0")}";

                if (player.Sail >= SailStatus.Rowing)
                {
                    movement = movement * player.RowingPower * Raylib.GetFrameTime();
                }

                switch (player.Sail)
                {
                    case SailStatus.Closed:
                        movement = new Vector2(0, 0);
                        break;
                    case SailStatus.Half:
                        movement += wind.WindDirection * wind.WindStrength / forceApplied / 2 * Raylib.GetFrameTime();
                        break;
                    case SailStatus.Full:
                        movement += wind.WindDirection * wind.WindStrength / forceApplied * Raylib.GetFrameTime();
                        break;
                }
                var adjustedPosition = sprite.Position
                    with
                {
                    X = sprite.Position.X,
                    Y = sprite.Position.Y,
                };

                if (singleton.Debug >= DebugLevel.Low)
                    Raylib.DrawCircle((int)adjustedPosition.X, (int)adjustedPosition.Y, 50f, Raylib.ORANGE);

                var newPosition = adjustedPosition + movement * player.Speed * Raylib.GetFrameTime();

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
                    //if (collidingTile != null)
                    //break;
                }
                if (collidingTile != null)
                {
                    if (collidingTile.Collision == CollisionType.Slow)
                    {
                        sprite.Position += movement * (player.Speed / 2) * Raylib.GetFrameTime();
                        //Console.WriteLine("I am on a slow tile");
                    }
                    else if (collidingTile.Collision == CollisionType.None)
                    {
                        sprite.Position += movement * player.Speed * Raylib.GetFrameTime();
                        //Console.WriteLine("Smooth sailing");
                    }
                    else if (collidingTile.Collision == CollisionType.Solid)
                    {
                        //Console.WriteLine($"Collision {collidingTile.Collision}");
                    }
                }
            });
        }
    }
}
