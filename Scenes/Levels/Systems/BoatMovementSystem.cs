using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Diagnostics;
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

            var windQueryDescription = new QueryDescription().WithAll<Wind>();
            Wind wind = null;
            var windQuery = world.Query(in windQueryDescription);
            foreach (var chunk in windQuery.GetChunkIterator())
            {
                foreach (var entity in chunk)
                {
                    wind = chunk.Get<Wind>(entity);
                    break;
                }
            }

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

                movement = RayMath.Vector2Rotate(movement, sprite.RotationAsRadians);

                var boatAngle = (sprite.Rotation + 360) % 360;
                var windAngle = (float)(((Math.Atan2(wind.WindDirection.Y, wind.WindDirection.X) * 180 / Math.PI) + 360 + 90) % 360);
                var angleDiff = Math.Abs(boatAngle - windAngle);

                //Console.WriteLine($"{boatAngle.ToString("0.0")} - Wind: {windAngle.ToString("0.0")} diff: {angleDiff.ToString("0.0")}");
                switch (player.Sail)
                {
                    case SailStatus.Closed:
                        movement = new Vector2(0, 0);
                        break;
                    case SailStatus.Half:
                        movement += wind.WindDirection * wind.WindStrength / angleDiff / 2 * Raylib.GetFrameTime();
                        break;
                    case SailStatus.Full:
                        movement += wind.WindDirection * wind.WindStrength / angleDiff * Raylib.GetFrameTime();
                        break;
                }
                var newPosition = sprite.Position + movement * player.Speed * Raylib.GetFrameTime();

                var query = world.Query(in tiles);

                Render collidingTile = null;
                foreach (ref var chunk in query.GetChunkIterator())
                {
                    foreach (var tile in chunk)
                    {
                        var tileSprite = chunk.Get<Render>(tile);

                        if (Raylib.CheckCollisionPointRec(newPosition, tileSprite.Destination))
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
