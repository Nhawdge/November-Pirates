using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Utilities;
using Raylib_CsLo;
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
            explosionSprite.Rotation = Random.Shared.Next(0, 360);
            explosionEntity.Set(explosionSprite);

            var explosion = new Effect();
            explosion.Duration = 0.3f;
            explosionEntity.Set(explosion);

        }

        internal static void CreateFire(World world, Vector2 pos)
        {
            var effectEntity = world.Create<Sprite, Effect, LayerAir>();

            var effectSprite = new Sprite(TextureKey.Fire, "Assets/Art/fire1", 1f, true);


            var fireSize = Random.Shared.Next(1, 3);
            effectSprite.Play($"Fire{fireSize}");
            effectSprite.Position = pos;
            //effectSprite.Rotation = Random.Shared.Next(0, 360);
            effectEntity.Set(effectSprite);

            var explosion = new Effect();
            explosion.Duration = 1.5f;
            effectEntity.Set(explosion);
        }

        internal static void CreateTrail(World world, Vector2 pos, Vector2 motion)
        {
            var effectEntity = world.Create<Sprite, Effect, LayerWater>();

            var effectSprite = new Sprite(TextureKey.WhitePixel, "Assets/Art/whitepixel", 1f, true);
            effectSprite.Position = pos;
            effectSprite.Color = new Color(255, 255, 255, 64);
            effectEntity.Set(effectSprite);

            var effect = new Effect();
            effect.Duration = 1f;
            effect.Motion = motion;
            effectEntity.Set(effect);
        }
    }
}
