using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class InventorySystem : GameSystem
    {
        internal override void Update(World world) { }

        internal override void UpdateNoCamera(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            if (singleton.InventoryOpen)
            {
                Console.WriteLine("Inventory Open");
                Raylib.DrawText("Inventory", 0, 0, 24, Raylib.BLUE);
                Raylib.DrawRectangle(Raylib.GetScreenWidth() / 2 - 200, Raylib.GetScreenHeight() / 2 - 200, 400, 400, Raylib.Fade(Raylib.BLACK, 0.8f));
            }
        }
    }
}
