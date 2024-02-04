using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Scenes.Levels.DataManagers;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
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

                Raylib.DrawText($"Crew: {playerShip.Crew} / {ShipData.Instance.Data[$"{playerShip.BoatType}{Stats.InitialCrew}"]}", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Hull: {playerShip.HullHealth} / {ShipData.Instance.Data[$"{playerShip.BoatType}{Stats.HullHealth}"]} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Hull: {playerShip.SailHealth} / {ShipData.Instance.Data[$"{playerShip.BoatType}{Stats.SailHealth}"]} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;

                Raylib.DrawText($"Gold: {InventoryManager.Instance.Gold} ", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                heightOffset -= lineOffset;


                foreach (var invGroup in InventoryManager.Instance.Inventory.GroupBy(x => x.Name))
                {
                    Raylib.DrawText($"{invGroup.Key}: {invGroup.Count()}", rightAlignment, Raylib.GetScreenHeight() / 2 - heightOffset, 24, Raylib.WHITE);
                    heightOffset -= lineOffset;
                }


                var texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(playerShip));

                Raylib.DrawTexturePro(texture.Texture,
                    new Rectangle(0, 0, texture.Texture.width, texture.Texture.height),
                    new Rectangle(inventoryFrame.X / 2 + 50, inventoryFrame.Y / 2 + 50, (inventoryFrame.X / 2) - 100, inventoryFrame.Y - 100),
                    new Vector2(0, 0), 0, Raylib.WHITE);

            }
        }
    }
}
