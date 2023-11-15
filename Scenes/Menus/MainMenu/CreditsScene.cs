using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class CreditsScene : BaseScene
    {
        public CreditsScene()
        {
            Systems.Add(new MenuSystem());

            World.Create(new UiTitle { Text = @"Credits", Order = 1 });
            World.Create(new UiTitle { Text = @"Nhawdge - Code", Order = 2 });
            World.Create(new UiTitle { Text = @"Kaitlyn Schmidt - Game Design", Order = 3 });
            World.Create(new UiTitle { Text = @"Game Design and Project Management?", Order = 4 });
            World.Create(new UiTitle { Text = @"Barun Sinha - Sounds", Order = 5 });
            World.Create(new UiTitle { Text = @"UnicornGirlie - Art ", Order = 6 });
            World.Create(new UiTitle { Text = @"Kenney.nl - Pirate Asset Pack", Order = 7 });

            World.Create(new UiButton
            {
                Text = "Back",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = 9
            });
        }
    }
}
