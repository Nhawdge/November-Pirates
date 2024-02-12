using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class SpawningSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var spawnerQuery = new QueryDescription().WithAll<Port, Spawner>();
            var singleton = world.QueryFirst<Singleton>().Get<Singleton>();

            world.Query(in spawnerQuery, (entity) =>
            {
                var spawner = entity.Get<Spawner>();
                var port = entity.Get<Port>();
                var faction = port.Team;

                spawner.Elapsed += Raylib.GetFrameTime();
                if (singleton.Debug >= DebugLevel.Low)
                    Raylib.DrawText((spawner.SpawnTime - spawner.Elapsed).ToString("0.0"), port.Position.X, port.Position.Y, 24, Raylib.BLACK);

                if (spawner.Elapsed > spawner.SpawnTime)
                {
                    var nearby = singleton.Map.GetTilesInSquareRange(port.Position, 5).OrderBy(x => Random.Shared.Next());
                    var spawnTile = nearby.First(x => x.MovementCost == 1);

                    var spawnPurpose = NpcPurpose.Merchant;

                    spawner.Elapsed = 0;
                    EnemyBuilder.CreateEnemyShip(world, spawnTile.Position, spawner.Team, spawnPurpose);
                }
            });
        }
    }
}
