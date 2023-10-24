using Arch.Core.Extensions;
using NovemberPirates.Scenes.Levels;
using NovemberPirates.Scenes.Menus.Components;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class MainMenuScene : BaseScene
    {
        public MainMenuScene()
        {
            Systems.Add(new MainMenuSystem());

            var title = World.Create<UiTitle>();

            var uiTitle = new UiTitle() { Text = "November Pirates" };
            title.Set(uiTitle);

            var button = World.Create<UiButton>();
            button.Set(new UiButton
            {
                Text= "Start Game",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new OceanScene();
                }
            });
        }
    }
}
