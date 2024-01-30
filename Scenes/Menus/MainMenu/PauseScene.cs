using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class PauseScene : BaseScene
    {
        public PauseScene(BaseScene lastScene)
        {
            Systems.Add(new MenuSystem());
            Systems.Add(new MenuMusicSystem());

            World.Create(new UiButton
            {
                Text = "Resume",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = lastScene;
                },
                Order = 1
            });

            World.Create(new UiButton
            {
                Text = "Settings",
                Action = () =>
                {
                    //NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = 2
            });

            World.Create(new UiButton
            {
                Text = "Main Menu",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = 3
            });

            World.Create(new UiButton
            {
                Text = "Exit Game",
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Order = 5
            });
        }
    }
}
