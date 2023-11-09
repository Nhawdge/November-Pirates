using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class EnemyBuilder
    {
        public static void CreateEnemyShip(World world, Vector2 position, Team team)
        {
            var entity = world.Create<Ship, Sprite>();
            var ship = new Ship();
            ship.Team = team;
            ship.BoatColor = BoatColor.Yellow;
            ship.BoatType = BoatType.HullLarge;
            ship.Sail = SailStatus.Full;
            ship.Crew = 10;

            var sprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            sprite.Position = position;
            entity.Set(ship);
            entity.Set(sprite);
        }

        internal static void CreatePatrolPoint(World world, Vector2 vector2, Team team, int order)
        {
            var patrolEntity = world.Create<PatrolPoint>();

            patrolEntity.Set(new PatrolPoint() { Position = vector2, Team = team, Order = order });
        }

        internal static void CreateSpawnPoint(World world, Vector2 pos, Team team)
        {
            var spawnerEntity = world.Create<Spawner>();

            spawnerEntity.Set(new Spawner() { Team = team, Position = pos, SpawnTime = 100, Elapsed = 100 });
        }
    }
}
