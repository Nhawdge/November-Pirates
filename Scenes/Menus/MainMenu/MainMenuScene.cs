using NovemberPirates.Components;
using NovemberPirates.Scenes.Levels;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class MainMenuScene : BaseScene
    {
        public MainMenuScene()
        {
            Systems.Add(new MenuSystem());

            //var title = World.Create<UiTitle>();
            //var uiTitle = new UiTitle() { Text = "November Pirates" };
            //title.Set(uiTitle);

            World.Create(new SpriteButton
            {
                Text = "Start",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new OceanScene();
                },
                Order = 1,
                TextSprite = new Sprite(TextureKey.Words, "Assets/Art/words")
                {
                    Position = new Vector2(750, 150),
                    OriginPos = Render.OriginAlignment.LeftTop
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(750, 150),
                    OriginPos = Render.OriginAlignment.LeftTop
                }
            });

            World.Create(new SpriteButton
            {
                Text = "HowToPlay",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new HowToPlayScene();
                },
                Order = 2,
                TextSprite = new Sprite(TextureKey.Words, "Assets/Art/words")
                {
                    Position = new Vector2(750, 300),
                    OriginPos = Render.OriginAlignment.LeftTop,
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(750, 300),
                    OriginPos = Render.OriginAlignment.LeftTop
                }
            });

            World.Create(new SpriteButton
            {
                Text = "Credits",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new CreditsScene();
                },
                Order = 3,
                TextSprite = new Sprite(TextureKey.Words, "Assets/Art/words")
                {
                    Position = new Vector2(750, 450),
                    OriginPos = Render.OriginAlignment.LeftTop
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(750, 450),
                    OriginPos = Render.OriginAlignment.LeftTop
                }
            });

            World.Create(new UiButton
            {
                Text = "Exit",
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Order = 5
            });
        }
    }
}
