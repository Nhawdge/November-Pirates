using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class CannonBallSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var cannonballQuery = new QueryDescription().WithAll<Cannonball, Sprite>();

        var allShips = new QueryDescription().WithAll<Ship, Sprite>();

            world.Query(in cannonballQuery, (entity) =>
            {
                var cannonball = entity.Get<Cannonball>();
                var sprite = entity.Get<Sprite>();

                sprite.Position += cannonball.Motion * Raylib.GetFrameTime();
            });
        }
    }
}
