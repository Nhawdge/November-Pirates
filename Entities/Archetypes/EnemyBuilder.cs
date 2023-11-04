using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class EnemyBuilder
    {
        public static void CreateEnemyShip(World world)
        {
            var entity = world.Create<Ship, Sprite>();
            var ship = new Ship();
            ship.BoatColor = BoatColor.Yellow;
            ship.BoatType = BoatType.HullLarge;
            ship.Sail = SailStatus.Closed;

            var sprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            sprite.Position = new System.Numerics.Vector2(3100, 3100);
            entity.Set(ship);
            entity.Set(sprite);
        }
    }
}
