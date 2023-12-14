using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class PlayerBuilder
    {
        internal static void Create(World world)
        {
            var player = world.Create<Player, Ship, Sprite>();

            var playerComponent = new Player();
            player.Set(playerComponent);

            var ship = new Ship(HullType.Small, BoatColor.Dead, Team.November);
            ship.Sail = SailStatus.Closed;

            ship.Cannons.Add(CannonBuilder.Create(Utilities.Data.CannonType.BFC1700, BoatSide.Starboard, 1));

            player.Set(ship);

            var playerSprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            playerSprite.Position = new System.Numerics.Vector2(3000, 3000);
            player.Set(playerSprite);
        }
    }
}
