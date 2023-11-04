using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class DebugSystem : GameSystem
    {
        internal override void Update(World world)
        {
        }
        internal override void UpdateNoCamera(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F2))
            {
                var newDebug = singleton.Debug + 1;
                if (newDebug > Enum.GetValues<DebugLevel>().Max())
                {
                    newDebug = 0;
                }

                singleton.Debug = newDebug;
            }
            if (singleton.Debug > DebugLevel.None)
            {
                var bottomLeft = new Rectangle(10, Raylib.GetScreenHeight() - 410, 200, 400);

                RayGui.GuiFade(0.75f);
                RayGui.GuiDummyRec(bottomLeft, "");
                RayGui.GuiFade(1f);

                RayGui.GuiSetStyle((int)GuiControl.LABEL, (int)GuiControlProperty.TEXT_ALIGNMENT, (int)GuiTextAlignment.TEXT_ALIGN_LEFT);
                RayGui.GuiSetStyle((int)GuiControl.LABEL, (int)GuiDefaultProperty.TEXT_SIZE, 24);

                RayGui.GuiLabel(bottomLeft, singleton.DebugText);
                singleton.DebugText = "";
            }
        }
    }
}
