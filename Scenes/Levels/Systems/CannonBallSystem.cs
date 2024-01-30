using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Scenes.Levels.Components;
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
                cannonball.Elapsed += Raylib.GetFrameTime();

                EffectsBuilder.CreateCannonTrail(world, sprite.Position);

                var end = sprite.Position;

                var destroyed = false;
                world.Query(in allShips, (shipEntity) =>
                {
                    var shipSprite = shipEntity.Get<Sprite>();
                    var ship = shipEntity.Get<Ship>();

                    if (cannonball.FiredBy == ship.Team)
                        return;

                    if (Raylib.CheckCollisionPointLine(shipSprite.Position, start, end, 50))
                    {
                        ship.HullHealth -= 5;
                        EffectsBuilder.CreateExplosion(world, end);
                        world.Create(new AudioEvent
                        {
                            Key = AudioKey.CannonHitShip,
                            Position = sprite.Position
                        });
                        destroyed = true;
                        ship.BoatCondition = ship.HullHealth switch
                        {
                            < 0 => BoatCondition.Empty,
                            < 25 => BoatCondition.Broken,
                            < 50 => BoatCondition.Torn,
                            < 75 => BoatCondition.Good,
                            _ => BoatCondition.Good
                        };
                        var shouldSpawnWood = Random.Shared.Next(0, 100) < 30;
                        if (shouldSpawnWood)
                        {
                            PickupBuilder.CreateWood(world, sprite.Position);
                        }

                        shipSprite.Texture = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship)).Texture;
                    }
                });
                if (destroyed)
                    world.Destroy(entity);
                else if (cannonball.Elapsed > cannonball.Duration)
                {
                    world.Create(new AudioEvent() { Key = AudioKey.CannonHitWater, Position = sprite.Position });
                    world.Destroy(entity);
                }
            });
        }
    }
}
