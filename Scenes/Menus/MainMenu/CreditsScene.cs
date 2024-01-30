using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class CreditsScene : BaseScene
    {
        public CreditsScene()
        {
            Systems.Add(new MenuSystem());
            Systems.Add(new MenuMusicSystem());

            var index = 5;
            World.Create(new UiContainer { Rectangle = new Rectangle() });

            World.Create(new UiTitle { Text = @"Credits", Order = index++ });
            World.Create(new UiTitle { Text = @"Nhawdge - Code", Order = index++ });
            World.Create(new UiTitle { Text = @"Kaitlyn Schmidt - Game Design", Order = index++ });
            World.Create(new UiTitle { Text = @"Barun Sinha - Sounds", Order = index++ });
            World.Create(new UiTitle { Text = @"UnicornGirlie - Title Art ", Order = index++ });
            World.Create(new UiTitle { Text = @"CFHM - Yarrs", Order = index++ });
            World.Create(new UiTitle { Text = @"Kenney.nl - Pirate Asset Pack", Order = index++ });

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
