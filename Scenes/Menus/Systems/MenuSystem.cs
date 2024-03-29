﻿using Arch.Core;
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
            var backgroundTexture = TextureManager.Instance.GetTexture(TextureKey.MainMenuBackground);
            Raylib.DrawTexturePro(backgroundTexture,
                new Rectangle(0, 0, backgroundTexture.width, backgroundTexture.height),
                new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
                Vector2.Zero, 0f, Raylib.WHITE);
            var query = new QueryDescription().WithAny<UiTitle, UiButton, SpriteButton, UiSlider>();
            var uiElementCount = world.CountEntities(in query);

            var centerPoint = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            var placementContainer = new Rectangle(centerPoint.X - 300, centerPoint.Y - 300, 600, 500);

            var container = world.QueryFirstOrNull<UiContainer>();
            if (container.HasValue)
            {
                var uiContainer = container.Value.Get<UiContainer>();

                //Raylib.DrawRectangleRec(container.Rectangle, Color.BLACK);
                RayGui.GuiDummyRec(uiContainer.Rectangle with
                {
                    X = placementContainer.x,
                    y = placementContainer.y,
                    width = placementContainer.width,
                    height = 55 * uiElementCount
                }, "");
            }

            //RayGui.GuiSetFont(CreditsFont);
            RayGui.GuiSetStyle((int)GuiControl.DEFAULT, (int)GuiDefaultProperty.TEXT_SIZE, 48);
            RayGui.GuiSetStyle((int)GuiControl.LABEL, (int)GuiControlProperty.TEXT_ALIGNMENT, 1);
            RayGui.GuiSetStyle((int)GuiControl.BUTTON, (int)GuiControlProperty.TEXT_ALIGNMENT, 0);

            world.Query(in query, (entity) =>
            {
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
                    var rect = placementContainer with
                    {
                        x = placementContainer.x + placementContainer.width / 2 - 200 / 2,
                        y = placementContainer.y + (60 * button.Order),
                        width = 200,
                        height = 50
                    };

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
                if (entity.Has<UiSlider>())
                {
                    var slider = entity.Get<UiSlider>();
                    var val = RayGui.GuiSlider(placementContainer with
                    {
                        x = placementContainer.x + placementContainer.width / 2 - 200 / 2,
                        y = placementContainer.y + (60 * slider.Order),
                        width = 200,
                        height = 50
                    },
                    slider.Text,
                    (SettingsManager.Instance.Settings[slider.SettingKey]).ToString("0") + "%",
                    SettingsManager.Instance.Settings[slider.SettingKey],
                    slider.MinValue, slider.MaxValue);

                    SettingsManager.Instance.Settings[slider.SettingKey] = val;

                }
            });
        }
    }
}
