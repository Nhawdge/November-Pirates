using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class EnemyBuilder
    {
        public static void CreateEnemyShip(World world, Vector2 position, Team team, NpcPurpose purpose)
        {
            var enemyArchetype = new ComponentType[] { typeof(Ship), typeof(Sprite), typeof(Npc) };

            var entity = world.Create(enemyArchetype);
            var color = team switch
            {
                Team.Red => BoatColor.Red,
                Team.Blue => BoatColor.Blue,
                Team.Yellow => BoatColor.Yellow,
                _ => BoatColor.Yellow,
            };
            var ship = new Ship(HullType.Small, color, Team.Yellow);

            ship.Team = team;

            ship.Sail = SailStatus.Closed;
            ship.Crew = 10;

            var sprite = ShipSpriteBuilder.GenerateBoat(new BoatOptions(ship));
            sprite.Position = position;

            sprite.RotationOffset = -90;
            entity.Set(ship);
            entity.Set(sprite);

            entity.Set(new Npc(purpose));

            //if (purpose == Purpose.Trade)
            //{
            //    ship.SailingRoute = RouteDataStore.Instance.GetRandomRoute();
            //    sprite.Position = ship.SailingRoute[0].RoutePoints[0];
            //}
        }

        internal static void CreatePatrolPoint(World world, Vector2 vector2, Team team, int order)
        {
            var patrolEntity = world.Create<PatrolPoint>();

            patrolEntity.Set(new PatrolPoint() { Position = vector2, Team = team, Order = order });
        }

        //internal static void CreateSpawnPoint(World world, Vector2 pos, Team team)
        //{
        //    var spawnerEntity = world.Create<Spawner>();

        //    spawnerEntity.Set(new Spawner() { Team = team,  SpawnTime = 100, Elapsed = 90 });
        //}
    }
}
