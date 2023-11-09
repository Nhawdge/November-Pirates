using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class SpawningSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var spawnerQuery = new QueryDescription().WithAll<Spawner>();

            world.Query(in spawnerQuery, (entity) =>
            {
                var spawner = entity.Get<Spawner>();
                spawner.Elapsed += Raylib.GetFrameTime();
                Raylib.DrawText((spawner.SpawnTime - spawner.Elapsed).ToString("0.0"), spawner.Position.X, spawner.Position.Y, 24, Raylib.BLACK);

                if (spawner.Elapsed > spawner.SpawnTime)
                {
                    spawner.Elapsed = 0;
                    EnemyBuilder.CreateEnemyShip(world, spawner.Position, spawner.Team);
                }
            });
        }
    }
}
