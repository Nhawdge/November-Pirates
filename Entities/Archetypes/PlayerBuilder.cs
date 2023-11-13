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

            var ship = new Ship();
            ship.Team = Team.Player;
            ship.BoatColor = BoatColor.Dead;
            ship.BoatType = BoatType.HullMedium;
            ship.Sail = SailStatus.Closed;
            ship.Crew = 10;
            ship.RowingPower = 150f;
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Port, Row = 1 });
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Starboard, Row = 1 });
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Port, Row = 2 });
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Starboard, Row = 2 });
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Port, Row = 3 });
            ship.Cannons.Add(new Cannon { Placement = BoatSide.Starboard, Row = 3 });

            player.Set(ship);

            var playerSprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            playerSprite.Position = new System.Numerics.Vector2(3000, 3000);
            player.Set(playerSprite);


        }
    }
}
