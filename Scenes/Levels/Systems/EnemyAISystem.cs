using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using NovemberPirates.Systems;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyAISystem : GameSystem
    {
        internal override void Update(World world)
        {
            var enemyQuery = new QueryDescription().WithAll<Sprite, Ship, Npc>();

            world.Query(enemyQuery, (entity) =>
            {
                var npc = entity.Get<Npc>();
                var ship = entity.Get<Ship>();
                var sprite = entity.Get<Sprite>();
                


            });

        }
    }


}
