using NovemberPirates.Components;
using NovemberPirates.Scenes.Levels;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class MainMenuScene : BaseScene
    {
        public MainMenuScene()
        {
            Systems.Add(new MenuSystem());
            Systems.Add(new MenuMusicSystem());

            World.Create(new Singleton() { Music = AudioKey.DreamingOfTreasure });

            var width = Raylib.GetScreenWidth() * 0.7f;

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
                    Position = new Vector2(width, 150),
                    OriginPos = Render.OriginAlignment.LeftTop
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(width, 150),
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
                    Position = new Vector2(width, 300),
                    OriginPos = Render.OriginAlignment.LeftTop,
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(width, 300),
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
                    Position = new Vector2(width, 450),
                    OriginPos = Render.OriginAlignment.LeftTop
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(width, 450),
                    OriginPos = Render.OriginAlignment.LeftTop
                }
            });

            World.Create(new SpriteButton
            {
                Text = "Exit",
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Order = 5,
                TextSprite = new Sprite(TextureKey.Words, "Assets/Art/words")
                {
                    Position = new Vector2(width, 750),
                    OriginPos = Render.OriginAlignment.LeftTop
                },
                ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                {
                    Position = new Vector2(width, 750),
                    OriginPos = Render.OriginAlignment.LeftTop
                }
            });
        }
    }
}
