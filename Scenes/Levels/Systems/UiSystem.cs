using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Menus.MainMenu;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class UiSystem : GameSystem
    {
        internal override void Update(World world)
        {
        }

        internal override void UpdateNoCamera(World world)
        {
            var playerEntity = world.QueryFirst<Player>();
            var player = playerEntity.Get<Player>();
            var ship = playerEntity.Get<Ship>();

            var topleft = new Rectangle(10, 10, 50, 50);
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            if (singleton.Debug > DebugLevel.None)
            {
                RayGui.GuiLabel(topleft, Enum.GetName<SailStatus>(ship.Sail) + " " + ship.BoatType);
                Raylib.DrawText(Raylib.GetFrameTime().ToString(), 10, 70, 20, Raylib.RED);
                Raylib.DrawFPS(10, 90);
            }


            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {
                NovemberPiratesEngine.Instance.ActiveScene = new PauseScene(NovemberPiratesEngine.Instance.ActiveScene);
            }
        }
    }
}
