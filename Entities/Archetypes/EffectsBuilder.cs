using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class EffectsBuilder
    {
        internal static void CreateExplosion(World world, Vector2 pos)
        {
            var explosionEntity = world.Create<Sprite, Effect>();

            var explosionSprite = new Sprite(TextureKey.Explosion, "Assets/Art/explosion", 1f, true);
            explosionSprite.Position = pos;
            explosionEntity.Set(explosionSprite);

            var explosion = new Effect();
            explosion.Duration = 0.3f;
            explosionEntity.Set(explosion);

        }
    }
}
