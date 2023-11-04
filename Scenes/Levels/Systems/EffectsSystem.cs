using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EffectsSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var effectsQuery = new QueryDescription().WithAll<Sprite, Effect>();

            world.Query(in effectsQuery, (effectEntity) =>
            {
                var effect = effectEntity.Get<Effect>();
                var sprite = effectEntity.Get<Sprite>();

                effect.Elapsed += Raylib.GetFrameTime();
                if (effect.Elapsed > effect.Duration)
                {
                    world.Destroy(effectEntity);
                }
            });
        }
    }
}
