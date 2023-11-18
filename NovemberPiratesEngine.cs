using NovemberPirates.Scenes;
using NovemberPirates.Scenes.Menus.MainMenu;
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

            var width = Raylib.GetMonitorWidth(0);
            var height = Raylib.GetMonitorHeight(0);

            Raylib.InitWindow(width, height, "November Pirates");
            Raylib.SetTargetFPS(60);
            Raylib.InitAudioDevice();
            Raylib.SetExitKey(0);
            Camera = new Camera2D
            {
                zoom = 1.0f,
                offset = new System.Numerics.Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
            };

            ActiveScene = new MainMenuScene();

            var data = ShipData.Instance;

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
