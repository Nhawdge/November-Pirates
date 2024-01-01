using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class WindSystem : GameSystem
    {
        internal override void Update(World world) { }

        internal override void UpdateNoCamera(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>(); 

            var wind = singletonEntity.Get<Wind>();
            wind.LastWindChange += Raylib.GetFrameTime();
            var windOffset = wind.LastWindChange - 75;
            if (windOffset > 0)
            {
                var shouldChange = Random.Shared.Next(0, Math.Max(90 - (int)windOffset, 1)) <= 1;
                if (shouldChange)
                {
                    wind.LastWindChange = 0;
                    wind.WindDirection = RayMath.Vector2Rotate(wind.WindDirection, Random.Shared.Next(0, 10));
                    wind.WindStrength = Random.Shared.Next(200, 400);
                    EffectsBuilder.CreateWindEffect(world);
                }
            }

            Raylib.DrawLine(50, 50, (int)(50 + wind.WindDirection.X * 30), (int)(50 + wind.WindDirection.Y * 30), Raylib.ORANGE);
        }
    }
}
