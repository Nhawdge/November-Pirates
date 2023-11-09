using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyControlSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var wind = singletonEntity.Get<Wind>();

            var enemyQuery = new QueryDescription().WithAll<Sprite, Ship>();
            var patrolQuery = new QueryDescription().WithAll<PatrolPoint>();

            world.Query(in enemyQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var ship = entity.Get<Ship>();
                var shouldMakeNewFire = Random.Shared.Next(0, 100) < 5;

                if (ship.BoatCondition == Utilities.BoatCondition.Broken && shouldMakeNewFire)
                {
                    EffectsBuilder.CreateFire(world, sprite.Position + new Vector2(Random.Shared.Next(-30, 30), Random.Shared.Next(-50, 30)));
                    if (ship.Crew > 0 && Random.Shared.Next(0, 100) < 5)
                    {
                        ship.Crew -= 1;
                        ship.Health += 1;
                        PickupBuilder.CreateCrewMember(world, sprite.Position);
                    }
                }
                if (ship.BoatCondition == Utilities.BoatCondition.Empty)
                {
                    sprite.Position += wind.WindDirection * Raylib.GetFrameTime() * wind.WindStrength * .1f;
                }
                if (ship.Health <= -100)
                {
                    world.Destroy(entity);
                }

                ship.NextPatrolPoint = 1;
                ship.Target = Vector2.Zero;

                var nextPoint = Vector2.Zero;
                world.Query(in patrolQuery, (patrolEntity) =>
                {
                    var point = patrolEntity.Get<PatrolPoint>();

                    if (point.Order == ship.NextPatrolPoint && (sprite.Position - point.Position).Length() < 10)
                    {
                        ship.NextPatrolPoint += 1;
                    }

                    if (point.Order == ship.NextPatrolPoint)
                    {
                        nextPoint = point.Position;
                    }
                });

            });
        }
    }
}
