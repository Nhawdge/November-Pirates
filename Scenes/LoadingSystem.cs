using Arch.Core;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Diagnostics;
using System.Numerics;

namespace NovemberPirates.Scenes
{
    internal class LoadingSystem : GameSystem
    {
        private BaseScene Scene;
        private int CurrentLoading;
        private bool LoadingComplete;
        private Stopwatch Stopwatch = new();
        private LoadingPhase Phase = LoadingPhase.Text;
        public LoadingSystem(BaseScene baseScene)
        {
            this.Scene = baseScene;
        }

        internal override void Update(World world)
        {
        }

        internal override void UpdateNoCamera(World world)
        {
            if (LoadingComplete || Scene.LoadingTasks.Count() == 0)
                return;
            var next = Scene.LoadingTasks.ElementAtOrDefault(CurrentLoading);
            if (next.Value is not null)
            {
                var center = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
                var text = $"Loading {next.Key} ({CurrentLoading + 1} of {Scene.LoadingTasks.Count()})";
                var size = 48;
                var width = Raylib.MeasureText(text, size);
                if (Phase == LoadingPhase.Text)
                {
                    Raylib.DrawText(text, center.X - width / 2, center.Y, size, Raylib.BLUE);
                    Console.WriteLine(text);
                    Phase = LoadingPhase.Load;
                }
                else if (Phase == LoadingPhase.Load)
                {
                    Stopwatch = Stopwatch.StartNew();
                    next.Value();
                    Phase = LoadingPhase.Text;
                    CurrentLoading++;
                    Console.WriteLine($"Done - {Stopwatch.ElapsedMilliseconds}ms ");
                }
            }
            else
                LoadingComplete = true;

        }
        private enum LoadingPhase
        {
            Text,
            Load
        }
    }
}