using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class PlayerBuilder
    {
        internal static void Create(World world, Vector2 startPos)
        {
            var player = world.Create<Player, Ship, Sprite>();

            var playerComponent = new Player();
            player.Set(playerComponent);

            var ship = new Ship(HullType.Large, BoatColor.Dead, Team.November);
            ship.Sail = SailStatus.Closed;

            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Starboard, 1));
            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Port, 1));
            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Starboard, 2));
            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Port, 2));
            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Starboard, 3));
            ship.Cannons.Add(CannonBuilder.Create(ship.HullType, Utilities.Data.CannonType.TrustyRusty, BoatSide.Port, 3));

            player.Set(ship);

            var playerSprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            playerSprite.Position = startPos;
            player.Set(playerSprite);

            NovemberPiratesEngine.Instance.Camera.target.X = playerSprite.Position.X;
            NovemberPiratesEngine.Instance.Camera.target.Y = playerSprite.Position.Y;
        }
    }
}
