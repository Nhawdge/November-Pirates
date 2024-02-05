using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class PlayerControlSystem : GameSystem
    {
        private Dictionary<ControlModes, List<Control>> Controls = new();
        internal override void Update(World world)
        {
            if (Controls.Count == 0)
            {
                LoadControls(world);
            }

            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var controlMode = singleton.InventoryOpen ? ControlModes.Inventory : ControlModes.Standard;

            var playerEntity = world.QueryFirst<Player>();

            var sprite = playerEntity.Get<Sprite>();
            var playerShip = playerEntity.Get<Ship>();

            var boatChanged = false;

            foreach (var control in Controls[controlMode])
            {
                if (control.RepeatPress)
                {
                    if (Raylib.IsKeyDown(control.Key))
                    {
                        boatChanged = control.Action() || boatChanged;
                    }
                }
                else
                {
                    if (Raylib.IsKeyPressed(control.Key))
                    {
                        boatChanged = control.Action() || boatChanged;
                    }
                }
            }

            if (singleton.Debug > DebugLevel.None)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_UP))
                {
                    playerShip.HullType = (HullType)Math.Min(Enum.GetValues<HullType>().Length - 1, (int)playerShip.HullType + 1);
                    boatChanged = true;

                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_PAGE_DOWN))
                {
                    playerShip.HullType = (HullType)Math.Max(0, (int)playerShip.HullType - 1);
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
        public void LoadControls(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            Controls.Add(ControlModes.Standard, new List<Control>());
            Controls.Add(ControlModes.Inventory, new List<Control>());

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_W,
                RepeatPress = false,
                Action = () =>
                {
                    var playerEntity = world.QueryFirst<Player>();
                    var sprite = playerEntity.Get<Sprite>();
                    var playerShip = playerEntity.Get<Ship>();

                    var priorSail = playerShip.Sail;
                    playerShip.Sail = (SailStatus)Math.Min(Enum.GetValues<SailStatus>().Length - 1, (int)playerShip.Sail + 1);

                    if (priorSail != playerShip.Sail && playerShip.Sail >= SailStatus.Half)
                    {
                        world.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.SailOpen, Position = sprite.Position });
                        return true;
                    }

                    return false;
                }
            });

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_S,
                RepeatPress = false,
                Action = () =>
                {
                    var playerEntity = world.QueryFirst<Player>();
                    var sprite = playerEntity.Get<Sprite>();
                    var playerShip = playerEntity.Get<Ship>();

                    var priorSail = playerShip.Sail;
                    playerShip.Sail = (SailStatus)Math.Max(0, (int)playerShip.Sail - 1);

                    if (priorSail != playerShip.Sail && playerShip.Sail >= SailStatus.Rowing)
                    {
                        world.Create<AudioEvent>().Set(new AudioEvent() { Key = AudioKey.SailClose, Position = sprite.Position });
                        return true;
                    }
                    return false;
                }
            });

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_A,
                RepeatPress = true,
                Action = () =>
                {
                    var playerEntity = world.QueryFirst<Player>();
                    var sprite = playerEntity.Get<Sprite>();
                    var playerShip = playerEntity.Get<Ship>();

                    sprite.Rotation -= playerShip.RotationSpeed * Raylib.GetFrameTime();
                    return false;
                }
            });

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_D,
                RepeatPress = true,
                Action = () =>
                {
                    var playerEntity = world.QueryFirst<Player>();
                    var sprite = playerEntity.Get<Sprite>();
                    var playerShip = playerEntity.Get<Ship>();

                    sprite.Rotation += playerShip.RotationSpeed * Raylib.GetFrameTime();
                    return false;
                }
            });


            var fireLeftCannon = () =>
            {
                var playerEntity = world.QueryFirst<Player>();
                var sprite = playerEntity.Get<Sprite>();
                var playerShip = playerEntity.Get<Ship>();

                // fire cannon Port
                var nextCannon = playerShip.Cannons.FirstOrDefault(x => x.ReloadElapsed >= x.ReloadTime && x.Placement == BoatSide.Port);

                if (nextCannon is not null)
                {
                    nextCannon.ReloadElapsed = 0f;
                    var cannonPos = sprite.Position + RayMath.Vector2Rotate(nextCannon.Position, sprite.RotationAsRadians);
                    Raylib.DrawCircleV(cannonPos, 10, Raylib.RED);

                    CannonballBuilder.Create(world, nextCannon, cannonPos, sprite.RenderRotation + 180, playerShip.Team);

                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = cannonPos, Key = AudioKey.CannonFire });
                }
                return false;
            };

            var fireRightCannon = () =>
            {
                var playerEntity = world.QueryFirst<Player>();
                var sprite = playerEntity.Get<Sprite>();
                var playerShip = playerEntity.Get<Ship>();

                var nextCannon = playerShip.Cannons.FirstOrDefault(x => x.ReloadElapsed >= x.ReloadTime && x.Placement == BoatSide.Starboard);

                if (nextCannon is not null)
                {
                    nextCannon.ReloadElapsed = 0f;
                    var cannonPos = sprite.Position + RayMath.Vector2Rotate(nextCannon.Position, sprite.RotationAsRadians);
                    //Raylib.DrawCircleV(cannonPos, 10, Raylib.RED);

                    CannonballBuilder.Create(world, nextCannon, cannonPos, sprite.RenderRotation, playerShip.Team);

                    var sound = world.Create<AudioEvent>();
                    sound.Set(new AudioEvent() { Position = cannonPos, Key = AudioKey.CannonFire });
                }
                return false;
            };

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_Q,
                RepeatPress = false,
                Action = fireLeftCannon
            });

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_LEFT,
                RepeatPress = true,
                Action = fireLeftCannon
            });


            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_E,
                RepeatPress = false,
                Action = fireRightCannon
            });

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_RIGHT,
                RepeatPress = true,
                Action = fireRightCannon
            });

            var openInventory = () =>
            {
                singleton.InventoryOpen = !singleton.InventoryOpen;
                return false;
            };

            Controls[ControlModes.Standard].Add(new Control()
            {
                Key = KeyboardKey.KEY_TAB,
                RepeatPress = false,
                Action = openInventory
            });

            Controls[ControlModes.Inventory].Add(new Control()
            {
                Key = KeyboardKey.KEY_TAB,
                RepeatPress = false,
                Action = openInventory
            });
        }
    }

    public class Control
    {
        public KeyboardKey Key;
        public Func<bool> Action = () => { return false; };
        public bool RepeatPress;
    }

    public enum ControlModes
    {
        Standard,
        Inventory,
    }
}
