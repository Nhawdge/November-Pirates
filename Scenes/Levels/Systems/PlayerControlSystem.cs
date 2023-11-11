using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class PlayerControlSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var playerEntity = world.QueryFirst<Player>();
            //var player = playerEntity.Get<Player>();

            var sprite = playerEntity.Get<Sprite>();
            var playerShip = playerEntity.Get<Ship>();

            var boatChanged = false;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
            {
                playerShip.Sail = (SailStatus)Math.Min(Enum.GetValues<SailStatus>().Length - 1, (int)playerShip.Sail + 1);
                boatChanged = true;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                playerShip.Sail = (SailStatus)Math.Max(0, (int)playerShip.Sail - 1);
                boatChanged = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                sprite.Rotation -= playerShip.RotationSpeed * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                sprite.Rotation += playerShip.RotationSpeed * Raylib.GetFrameTime();
            }

            if (boatChanged)
                sprite.Texture = (ShipSpriteBuilder.GenerateBoat(new BoatOptions(BoatType.HullLarge, BoatColor.Dead, playerShip.Sail, playerShip.BoatCondition))).Texture;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                // fire cannon Port
                CannonballBuilder.Create(world, sprite.Position, sprite.RenderRotation + 180, Team.Player);
                var sound = world.Create<AudioEvent>();
                sound.Set(new AudioEvent() { Position = sprite.Position, Key = AudioKey.CannonFire });
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_E) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                // fire cannon Starboard
                CannonballBuilder.Create(world, sprite.Position, sprite.RenderRotation, Team.Player);
                var sound = world.Create<AudioEvent>();
                sound.Set(new AudioEvent() { Position = sprite.Position, Key = AudioKey.CannonFire });
            }

        }
    }
}
