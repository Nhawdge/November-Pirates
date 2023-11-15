using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Menus.Systems
{
    internal class MenuSystem : GameSystem
    {
        internal override void Update(World world) { }

        internal override void UpdateNoCamera(World world)
        {
            Raylib.DrawTexture(TextureManager.Instance.GetTexture(TextureKey.MainMenuBackground), 0, 0, Raylib.WHITE);
            var query = new QueryDescription().WithAny<UiTitle, UiButton, SpriteButton>();

            var centerPoint = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            var dummyrect = new Rectangle(centerPoint.X - 200, centerPoint.Y - 300, 400, 500);
            //RayGui.GuiDummyRec(dummyrect, "");
            var index = 0;

            RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiDefaultProperty.TEXT_SIZE, 48);
            RayGui.GuiSetStyle((int)GuiControl.LABEL, (int)GuiControlProperty.TEXT_ALIGNMENT, 1);

            RayGui.GuiSetStyle((int)GuiControl.BUTTON, (int)GuiControlProperty.TEXT_ALIGNMENT, 0);
            world.Query(in query, (entity) =>
            {
                index++;
                if (entity.Has<UiTitle>())
                {
                    var titleComponent = entity.Get<UiTitle>();

                    var text = titleComponent.Text;
                    var rect = new Rectangle(centerPoint.X - 100, 0 + 50 * titleComponent.Order, 200, 100);

                    RayGui.GuiLabel(rect, text);
                }
                RayGui.GuiSetStyle((int)GuiControl.BUTTON, (int)GuiControlProperty.TEXT_ALIGNMENT, 1);

                RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiDefaultProperty.TEXT_SIZE, 24);

                if (entity.Has<UiButton>())
                {
                    var button = entity.Get<UiButton>();
                    var rect = dummyrect with { x = dummyrect.x + 100, y = dummyrect.y + (60 * button.Order), width = 200, height = 50 };

                    if (RayGui.GuiButton(rect, button.Text))
                    {
                        button.Action();
                    }
                }
                if (entity.Has<SpriteButton>())
                {
                    var button = entity.Get<SpriteButton>();

                    button.ButtonSprite.Play("Normal");
                    button.TextSprite.Play(button.Text);
                    if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), button.ButtonSprite.Destination))
                    {
                        button.ButtonSprite.Play("Hover");
                        button.TextSprite.Play($"{button.Text}Hover");
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        {
                            button.Action();
                        }
                    }
                    button.ButtonSprite.Draw();
                    button.TextSprite.Draw();
                }
            });
        }
    }
}
