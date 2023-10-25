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

            world.Query(sprites, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var player = entity.Get<Player>();

                var movement = new Vector2(0, 0);

                if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                {
                    movement.Y -= 1;
                }
                if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                {
                    movement.Y += 1;
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

                var newPosition = sprite.Position + movement * player.Speed * Raylib.GetFrameTime();

                var query = world.Query(in tiles);
                var collides = false;

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
                        Console.WriteLine("I am on a slow tile");
                    }
                    else if (collidingTile.Collision == CollisionType.None)
                    {
                        sprite.Position += movement * player.Speed * Raylib.GetFrameTime();
                        Console.WriteLine("Smooth sailing");
                    }
                    else if (collidingTile.Collision == CollisionType.Solid)
                    {
                        Console.WriteLine($"Collision {collidingTile.Collision}");
                    }
                }
            });
        }
    }
}
