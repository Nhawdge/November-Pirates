using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class HowToPlayScene : BaseScene
    {
        public HowToPlayScene()
        {
            Systems.Add(new MenuSystem());

            var instructions = World.Create<UiTitle>();
            instructions.Set(new UiTitle { Text = @"WASD to move Q/E or Arrow keys to shoot", Order = 1 });
            var instructions2 = World.Create<UiTitle>();
            instructions2.Set(new UiTitle { Text = @"f3 to change the wind", Order = 2 });
            var instructions3 = World.Create<UiTitle>();
            instructions3.Set(new UiTitle { Text = @"F2 for the debugger I'm proud of", Order = 3 });

            World.Create(new UiButton
            {
                Text = "Back",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = 5
            });
        }

    }
}
