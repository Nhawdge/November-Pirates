using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Systems;
using Raylib_CsLo;
using System.Numerics;

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

                if (effect.Wiggle)
                {
                    // TODO Figure this math out
                    var angle = Math.Atan2(effect.Motion.Y, effect.Motion.X);
                    var offset = new Vector2();
                    var elapsed = effect.Elapsed;
                    offset.Y = Math.Abs((float)Math.Abs(Math.Sin(elapsed)));
                    var rotatedOffset = RayMath.Vector2Rotate(offset, (float)angle);

                    sprite.Position += (effect.Motion + rotatedOffset) * Raylib.GetFrameTime();
                }
                else
                {
                    sprite.Position += effect.Motion * Raylib.GetFrameTime();
                }

                if (effect.Fadeout)
                {
                    var percentFade = effect.FadeStart - (effect.Elapsed / effect.Duration);
                    sprite.Color = Raylib.Fade(sprite.Color, percentFade);
                }

                effect.Elapsed += Raylib.GetFrameTime();
                if (effect.Elapsed > effect.Duration)
                {
                    world.Destroy(effectEntity);
                }
                if (effect.CreateTrail)
                {
                    EffectsBuilder.CreateAirTrail(world, sprite.Position, Vector2.Zero, false);
                }

            });
        }
    }
}
