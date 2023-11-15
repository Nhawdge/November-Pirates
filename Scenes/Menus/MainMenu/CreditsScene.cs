using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class CreditsScene : BaseScene
    {
        public CreditsScene()
        {
            Systems.Add(new MenuSystem());
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
