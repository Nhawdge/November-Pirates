﻿using Arch.Core;
using NovemberPirates.Scenes;
using NovemberPirates.Scenes.Menus.MainMenu;
using Raylib_CsLo;

namespace NovemberPirates
{
    internal class NovemberPiratesEngine
    {
        private NovemberPiratesEngine() { }

        public static NovemberPiratesEngine Instance = new NovemberPiratesEngine();

        public Camera2D Camera;
        internal BaseScene ActiveScene;

        public void StartGame()
        {

            Raylib.InitWindow(1280, 720, "November Pirates");
            Raylib.SetTargetFPS(60);

            Camera = new Camera2D
            {
                zoom = 1.0f
            };

            ActiveScene = new MainMenuScene();

            while (!Raylib.WindowShouldClose())
            {
                GameLoop();
            }
        }

        public void GameLoop()
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode2D(Camera);

            Raylib.ClearBackground(Raylib.BLACK);

            ActiveScene.Systems.ForEach(system => system.Update(ActiveScene.World));

            Raylib.EndMode2D();

            ActiveScene.Systems.ForEach(system => system.UpdateNoCamera(ActiveScene.World));
            Raylib.EndDrawing();
        }
    }
}
