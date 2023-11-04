using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyControlSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var enemyQuery = new QueryDescription().WithAll<Sprite, Ship>();

            world.Query(in enemyQuery, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                var ship = entity.Get<Ship>();
            });

            world.Query(enemyQuery);
        }
    }
}
