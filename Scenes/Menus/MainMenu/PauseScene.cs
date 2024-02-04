using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
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
            var singletonEntity = lastScene.World.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();
            World.Create(singleton);
            var order = 1;

            World.Create(new UiButton
            {
                Text = "Resume",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = lastScene;
                },
                Order = order++
            });

            World.Create(new UiButton
            {
                Text = "Settings",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new SettingsScene(singleton, lastScene);
                },
                Order = order++
            });

            World.Create(new UiButton
            {
                Text = "Main Menu",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new MainMenuScene();
                },
                Order = order++
            });

            World.Create(new UiButton
            {
                Text = "Exit Game",
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Order = order += 2
            });
        }
    }
}
