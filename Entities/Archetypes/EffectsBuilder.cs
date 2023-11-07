using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
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

            var effect = new Effect();
            effect.CreateTrail = true;

            effect.Duration = 1.5f;
            effectEntity.Set(effect);
        }

        internal static void CreateWaterTrail(World world, Vector2 pos, Vector2 motion)
        {
            var effectEntity = world.Create<Sprite, Effect, LayerWater>();

            var effectSprite = new Sprite(TextureKey.WhitePixel, "Assets/Art/whitepixel", 1f, true);
            effectSprite.Position = pos;
            effectSprite.Color = new Color(255, 255, 255, 64);
            effectEntity.Set(effectSprite);

            var effect = new Effect();
            effect.Fadeout = true;
            effect.Duration = 1f;
            effect.Motion = motion;
            effect.CreateTrail = false;
            effectEntity.Set(effect);
        }

        internal static void CreateAirTrail(World world, Vector2 pos, Vector2 motion, bool createTrail)
        {
            var effectEntity = world.Create<Sprite, Effect, LayerAir>();

            var effectSprite = new Sprite(TextureKey.WhitePixel, "Assets/Art/whitepixel", 1f, true);
            effectSprite.Position = pos;
            effectSprite.Color = new Color(255, 255, 255, 96);
            effectEntity.Set(effectSprite);

            var effect = new Effect();
            effect.Wiggle = true;
            effect.Fadeout = true;
            effect.FadeStart = 0.4f;
            effect.CreateTrail = createTrail;
            effect.Duration = (createTrail ? 5f : 1f) * 3;
            if (createTrail)
            {
                effect.WiggleTimerOffset = (float)Random.Shared.Next(0,2) ;
            }
            effect.Motion = motion;
            effectEntity.Set(effect);

            effectEntity.Set(new LayerAir());
        }

        internal static void CreateWindEffect(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var wind = singletonEntity.Get<Wind>();

            var centerScreen = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
            var spread = 800;

            for (int i = 0; i < 30; i++)
            {
                var centerPos = Raylib.GetScreenToWorld2D(centerScreen with
                {
                    X = centerScreen.X + Random.Shared.Next(-spread, spread),
                    Y = centerScreen.Y + Random.Shared.Next(-spread, spread)
                }, NovemberPiratesEngine.Instance.Camera);
                CreateAirTrail(world, centerPos, wind.WindDirection * 100, true);
            }

        }
    }
}
