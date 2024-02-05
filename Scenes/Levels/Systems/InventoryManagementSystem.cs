using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Scenes.Levels.DataManagers;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using NovemberPirates.Utilities.ContentData;
using NovemberPirates.Utilities.Data;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class InventoryManagementSystem : GameSystem
    {
        internal override void Update(World world) { }

        internal override void UpdateNoCamera(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var playerEntity = world.QueryFirst<Player>();
            var player = playerEntity.Get<Player>();

            if (singleton.InventoryOpen)
            {
                var playerShip = playerEntity.Get<Ship>();
                var realShipSprite = playerEntity.Get<Sprite>();
                var inventoryFrame = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

                Raylib.DrawText("Inventory", 0, 0, 24, Raylib.BLUE);

                Raylib.DrawRectangle(
                    (int)(Raylib.GetScreenWidth() / 2 - inventoryFrame.X / 2),
                    (int)(Raylib.GetScreenHeight() / 2 - inventoryFrame.Y / 2),
                    (int)inventoryFrame.X,
                    (int)inventoryFrame.Y,
                    Raylib.Fade(Raylib.BLACK, 0.8f));

                var rightAlignment = Raylib.GetScreenWidth() / 2 + 100;
                var leftAlignment = inventoryFrame.X + 10;
                var heightOffset = 180;
                var lineOffset = 30;

                // Left Side

                var shipSprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(playerShip));
                shipSprite.RotationOffset = 180;

                var leftCenter = new Vector2(leftAlignment - inventoryFrame.X / 4, inventoryFrame.Y);
                shipSprite.Position = leftCenter;
                shipSprite.Scale = 4f;
                shipSprite.Draw();

                var shipChanged = false;

                if (singleton.EquipModeActive)
                {
                    var maxRow = ShipData.Instance.Data[$"{playerShip.HullType}{Stats.MaxCannons}"] / 2;

                    for (int i = 1; i <= maxRow; i++)
                    {
                        var cannon = CannonBuilder.Create(playerShip.HullType, CannonType.TrustyRusty, BoatSide.Port, i);

                        var rect = new Rectangle(
                            (int)(leftCenter.X + (cannon.Placement == BoatSide.Port ? 40 : -120)),
                            (int)(leftCenter.Y + (cannon.Position.Y * 4) - 130),
                            70, 60);

                        Raylib.DrawRectangleLines((int)rect.X, (int)rect.Y, (int)rect.width, (int)rect.height, Raylib.BLACK);

                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
                        {
                            singleton.EquipModeActive = false;
                            playerShip.Cannons.Add(cannon);
                            InventoryManager.Instance.Inventory.Remove(InventoryManager.Instance.Inventory.First(x => x.EquipModeActive));
                            shipChanged = true || shipChanged;
                            break;
                        }

                        var cannon2 = CannonBuilder.Create(playerShip.HullType, CannonType.TrustyRusty, BoatSide.Starboard, i);
                        var rect2 = new Rectangle(
                          (int)(leftCenter.X + (cannon2.Placement == BoatSide.Port ? 40 : -120)),
                          (int)(leftCenter.Y + (cannon2.Position.Y * 4) - 130),
                          70, 60);

                        Raylib.DrawRectangleLines((int)rect2.X, (int)rect2.Y, (int)rect2.width, (int)rect2.height, Raylib.BLACK);

                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect2))
                        {
                            singleton.EquipModeActive = false;
                            playerShip.Cannons.Add(cannon2);
                            InventoryManager.Instance.Inventory.Remove(InventoryManager.Instance.Inventory.First(x => x.EquipModeActive));
                            shipChanged = true || shipChanged;
                            break;
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < playerShip.Cannons.Count; i++)
                    {
                        var cannon = playerShip.Cannons[i];

                        var rect = new Rectangle(
                            (int)(leftCenter.X + (cannon.Placement == BoatSide.Port ? 40 : -120)),
                            (int)(leftCenter.Y + (cannon.Position.Y * 4) - 130),
                            70, 60);

                        Raylib.DrawRectangleLines((int)rect.X, (int)rect.Y, (int)rect.width, (int)rect.height, Raylib.BLACK);

                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
                        {
                            playerShip.Cannons.Remove(cannon);
                            InventoryManager.Instance.Inventory.Add(TradeableGoodsData.GetGood((TradeableGoodsNames)cannon.CannonType));
                            shipChanged = true || shipChanged;
                        }
                    }
                }


                // Right Side 

                Raylib.DrawText($"Crew: {playerShip.Crew} / {ShipData.Instance.Data[$"{playerShip.HullType}{Stats.InitialCrew}"]}", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Hull: {playerShip.HullHealth} / {ShipData.Instance.Data[$"{playerShip.HullType}{Stats.HullHealth}"]} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Sails: {playerShip.SailHealth} / {ShipData.Instance.Data[$"{playerShip.HullType}{Stats.SailHealth}"]} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Gold: {InventoryManager.Instance.Gold} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;


                foreach (var invGroup in InventoryManager.Instance.Inventory.GroupBy(x => x.Name))
                {
                    Raylib.DrawText($"{invGroup.Key}: {invGroup.Count()}", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                    if (invGroup.Any(x => x.IsEquipable))
                    {
                        if (RayGui.GuiButton(new Rectangle(rightAlignment + 100, Raylib.GetScreenHeight() / 2 - heightOffset, 100, 30), "Equip"))
                        {
                            singleton.EquipModeActive = true;
                            invGroup.First().EquipModeActive = true;
                            //InventoryManager.Instance.Inventory.Remove(invGroup.First());
                        };
                    }


                    heightOffset -= lineOffset;
                }

                if (shipChanged)
                {
                    var newboat = ShipSpriteBuilder.GenerateBoat(new BoatOptions(playerShip));
                    newboat.Position = realShipSprite.Position;
                    newboat.Rotation = realShipSprite.Rotation;
                    playerEntity.Set(newboat);
                }
            }
        }
    }
}
