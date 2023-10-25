using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class MainMenuSystem : GameSystem
    {
        internal override void Update(World world)
        {
        }

        internal override void UpdateNoCamera(World world)
        {
            var query = new QueryDescription().WithAny<UiTitle, UiButton>();

            var centerPoint = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            var dummyrect = new Rectangle(centerPoint.X - 200, centerPoint.Y - 150, 400, 400);
            RayGui.GuiDummyRec(dummyrect, "");
            world.Query(in query, (entity) =>
            {
                if (entity.Has<UiTitle>())
                {
                    var titleComponent = entity.Get<UiTitle>();

                    var text = titleComponent.Text;
                    var rect = new Rectangle(centerPoint.X - 100, 200, 200, 100);
                    //RayGui.GuiTextBox(text, centerPoint.X - 200, centerPoint.Y - 200, 24, Raylib.ORANGE);

                    RayGui.GuiSetStyle((int)GuiControl.LABEL, (int)GuiControlProperty.TEXT_ALIGNMENT, 1);

                    // TODO fix font size
                    var font = RayGui.GuiGetFont();
                    font.baseSize = 48;
                    RayGui.GuiSetFont(font);

                    RayGui.GuiLabel(rect, text);
                }

                if (entity.Has<UiButton>())
                {
                    var button = entity.Get<UiButton>();
                    var rect = dummyrect with { x = dummyrect.x + 100, y = dummyrect.y + 50, width = 200, height = 60 };

                    if (RayGui.GuiButton(rect, button.Text))
                    {
                        button.Action();
                    }
                }
            });
        }
    }
}
