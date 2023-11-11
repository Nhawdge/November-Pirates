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

            var playerSprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(BoatType.HullLarge, BoatColor.Dead, SailStatus.Closed));
            playerSprite.Position = new System.Numerics.Vector2(3000, 3000);
            player.Set(playerSprite);

            var ship = new Ship();
            ship.Team = Team.Player;
            ship.BoatColor = BoatColor.Dead;
            ship.BoatType = BoatType.HullLarge;
            ship.Sail = SailStatus.Closed;
            ship.Crew = 10;
            player.Set(ship);

        }
    }
}
