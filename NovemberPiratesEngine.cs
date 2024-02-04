using NovemberPirates.Scenes;
using NovemberPirates.Scenes.Menus.MainMenu;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Data;
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
            //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TOPMOST);
            //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_MAXIMIZED);
            //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_UNDECORATED);

            Raylib.InitWindow(0, 0, "November Pirates");

            var monitor = Raylib.GetCurrentMonitor();
            var width = Raylib.GetMonitorWidth(monitor);
            var height = Raylib.GetMonitorHeight(monitor);

            Raylib.SetWindowSize(width, height);

            Raylib.SetTargetFPS(60);
            Raylib.InitAudioDevice();
            Raylib.SetExitKey(0);

            if (SettingsManager.Instance.Settings[SettingsManager.SettingKeys.Fullscreen] == 1)
            {
                Raylib.ToggleFullscreen();
            }

            Camera = new Camera2D
            {
                zoom = 1.0f,
                offset = new System.Numerics.Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
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
            for (int i = 0; i < ActiveScene.Systems.Count; i++)
            {
                ActiveScene.Systems[i].Update(ActiveScene.World);
            }

            Raylib.EndMode2D();

            for (int i = 0; i < ActiveScene.Systems.Count; i++)
            {
                ActiveScene.Systems[i].UpdateNoCamera(ActiveScene.World);
            }
            Raylib.EndDrawing();
        }
    }
}
