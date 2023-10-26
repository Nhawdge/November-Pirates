using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class WindSystem : GameSystem
    {
        internal override void Update(World world)
        {

        }

        internal override void UpdateNoCamera(World world)
        {
            var query = new QueryDescription().WithAll<Wind, Singleton>();

            world.Query(in query, (entity) =>
            {
                var wind = entity.Get<Wind>();
                var singleton = entity.Get<Singleton>();

                wind.WindDirection = RayMath.Vector2Rotate(wind.WindDirection, 0.1f * Raylib.GetFrameTime());

                Raylib.DrawLine(50, 50, (int)(50 + wind.WindDirection.X * 30), (int)(50 + wind.WindDirection.Y * 30), Raylib.ORANGE);
            });
        }
    }
}
