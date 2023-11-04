using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class CannonBallSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var cannonballQuery = new QueryDescription().WithAll<Cannonball, Sprite>();

            var allShips = new QueryDescription().WithAll<Ship, Sprite>();

            world.Query(in cannonballQuery, (entity) =>
            {
                var cannonball = entity.Get<Cannonball>();
                var sprite = entity.Get<Sprite>();

                var start = sprite.Position;
                sprite.Position += cannonball.Motion * Raylib.GetFrameTime();

                var end = sprite.Position;

                world.Query(in allShips, (shipEntity) =>
                {
                    var shipSprite = shipEntity.Get<Sprite>();
                    var ship = shipEntity.Get<Ship>();
                    if (Raylib.CheckCollisionPointLine(shipSprite.Position, start, end, 50))
                    {
                        ship.Health -= 5;
                        world.Destroy(entity);
                        EffectsBuilder.CreateExplosion(world, end);

                        ship.BoatCondition = ship.Health switch
                        {
                            < 0 => BoatCondition.Empty,
                            < 25 => BoatCondition.Broken,
                            < 50 => BoatCondition.Torn,
                            < 75 => BoatCondition.Good,
                            _ => BoatCondition.Good
                        };
                        ship.Sail = SailStatus.Full;

                        shipSprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;

                    }
                });
            });
        }
    }
}
