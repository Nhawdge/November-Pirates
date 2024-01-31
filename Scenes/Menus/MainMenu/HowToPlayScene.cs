using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class HowToPlayScene : BaseScene
    {
        public HowToPlayScene(Singleton singleton)
        {
            Systems.Add(new MenuSystem());
            Systems.Add(new MenuMusicSystem());

            World.Create(singleton);

            var index = 5;

            World.Create(new UiContainer { Rectangle = new Rectangle() });

            var instructions = World.Create<UiTitle>();
            instructions.Set(new UiTitle { Text = @"WASD to move Q/E or Arrow keys to shoot", Order = index++ });
            var instructions2 = World.Create<UiTitle>();
            instructions2.Set(new UiTitle { Text = @"f3 to change the wind", Order = index++ });
            var instructions3 = World.Create<UiTitle>();
            instructions3.Set(new UiTitle { Text = @"F2 for the debugger I'm proud of", Order = index++ });

            World.Create(new UiButton
            {
                Text = "Back",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = index++
            });
        }

    }
}
