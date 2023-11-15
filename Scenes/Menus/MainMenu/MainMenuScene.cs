using Arch.Core.Extensions;
using NovemberPirates.Scenes.Levels;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class MainMenuScene : BaseScene
    {
        public MainMenuScene()
        {
            Systems.Add(new MenuSystem());

            var title = World.Create<UiTitle>();

            var uiTitle = new UiTitle() { Text = "November Pirates" };
            title.Set(uiTitle);

            var button = World.Create<UiButton>();
            button.Set(new UiButton
            {
                Text = "Start Game",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new OceanScene();
                },
                Order = 1
            });


            World.Create(new UiButton
            {
                Text = "How to Play",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new HowToPlayScene();
                },
                Order = 2
            });

            World.Create(new UiButton
            {
                Text = "Credits",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new CreditsScene();
                },
                Order = 3
            });


            World.Create(new UiButton
            {
                Text = "Exit",
                Action = () =>
                {
                    Environment.Exit(0);
                    //NovemberPiratesEngine.Instance.ActiveScene = new HowToPlayScene();
                },
                Order = 5
            });



        }
    }
}
