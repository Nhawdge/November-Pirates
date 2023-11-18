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
                var priorSail = playerShip.Sail;
                playerShip.Sail = (SailStatus)Math.Min(Enum.GetValues<SailStatus>().Length - 1, (int)playerShip.Sail + 1);

                if (priorSail != playerShip.Sail && playerShip.Sail >= SailStatus.Half)
                    world.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.SailOpen, Position = sprite.Position });

                boatChanged = true;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                var priorSail = playerShip.Sail;
                playerShip.Sail = (SailStatus)Math.Max(0, (int)playerShip.Sail - 1);

                if (priorSail != playerShip.Sail && playerShip.Sail >= SailStatus.Rowing)
                    world.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.SailClose, Position = sprite.Position });

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



            if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                // fire cannon Port
                var nextCannon = playerShip.Cannons.FirstOrDefault(x => x.ReloadElapsed >= x.ReloadTime && x.Placement == BoatSide.Port);

                if (nextCannon is not null)
                {
                    nextCannon.ReloadElapsed = 0f;
                    var cannonPos = sprite.Position + RayMath.Vector2Rotate(nextCannon.Position, sprite.RotationAsRadians);
                    Raylib.DrawCircleV(cannonPos, 10, Raylib.RED);

                    CannonballBuilder.Create(world, nextCannon, cannonPos, sprite.RenderRotation + 180, Team.Player);

                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = cannonPos, Key = AudioKey.CannonFire });
                }
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_E) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                // fire cannon Starboard

                var nextCannon = playerShip.Cannons.FirstOrDefault(x => x.ReloadElapsed >= x.ReloadTime && x.Placement == BoatSide.Starboard);

                if (nextCannon is not null)
                {
                    nextCannon.ReloadElapsed = 0f;
                    var cannonPos = sprite.Position + RayMath.Vector2Rotate(nextCannon.Position, sprite.RotationAsRadians);
                    //Raylib.DrawCircleV(cannonPos, 10, Raylib.RED);

                    CannonballBuilder.Create(world, nextCannon, cannonPos, sprite.RenderRotation, Team.Player);

                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = cannonPos, Key = AudioKey.CannonFire });
                }
            }

            if (singleton.Debug > DebugLevel.None)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP))
                {
                    playerShip.BoatType = (HullType)Math.Min(Enum.GetValues<HullType>().Length - 1, (int)playerShip.BoatType + 1);
                    boatChanged = true;

                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN))
                {
                    playerShip.BoatType = (HullType)Math.Max(0, (int)playerShip.BoatType - 1);
                    boatChanged = true;

                }
            }
            if (boatChanged)
            {
                var newboat = ShipSpriteBuilder.GenerateBoat(new BoatOptions(playerShip));
                newboat.Position = sprite.Position;
                newboat.Rotation = sprite.Rotation;
                playerEntity.Set(newboat);

                //sprite.Texture = newboat.Texture;
            }
        }
    }
}
