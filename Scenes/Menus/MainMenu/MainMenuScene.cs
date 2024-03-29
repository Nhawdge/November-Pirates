﻿using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels;
using NovemberPirates.Scenes.Levels.Components;
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

            var singleton = new Singleton() { Music = AudioKey.DreamingOfTreasure };
            World.Create(singleton);

            var width = Raylib.GetScreenWidth() * 0.7f;
            var order = 1;

            World.Create(new SpriteButton
            {
                Text = "Start",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new OceanScene();
                },
                Order = order++,
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
                    NovemberPiratesEngine.Instance.ActiveScene = new HowToPlayScene(singleton);
                },
                Order = order++,
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
                    NovemberPiratesEngine.Instance.ActiveScene = new CreditsScene(singleton);
                },
                Order = order++,
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
            World.Create(new UiButton
            {
                Text = "Settings",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = new SettingsScene(singleton, this);
                },
                Order = order++,
                //TextSprite = new Sprite(TextureKey.Words, "Assets/Art/words")
                //{
                //    Position = new Vector2(width, 750),
                //    OriginPos = Render.OriginAlignment.LeftTop
                //},
                //ButtonSprite = new Sprite(TextureKey.Button, "Assets/Art/Button")
                //{
                //    Position = new Vector2(width, 750),
                //    OriginPos = Render.OriginAlignment.LeftTop
                //}
            });
            World.Create(new SpriteButton
            {
                Text = "Exit",
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Order = order += 2,
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
