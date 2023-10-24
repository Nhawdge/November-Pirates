using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace November_Pirates.Scenes.Levels.Systems
{
    internal class BoatMovementSystem : GameSystem
    {
        public BoatMovementSystem()
        {
        }

        internal override void Update(World world)
        {
            var sprites = new QueryDescription().WithAll<Player, Sprite>();

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

                sprite.Position += movement * player.Speed * Raylib.GetFrameTime();

            });
        }
    }
}
