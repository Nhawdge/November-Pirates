using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class PortSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var portQuery = new QueryDescription().WithAll<Port>();
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            world.Query(portQuery, (portEntity) =>
            {
                var port = portEntity.Get<Port>();
                port.Currency += 1f * Raylib.GetFrameTime();

                if (singleton.Debug >= DebugLevel.Low)
                {
                    Raylib.DrawText($"Port Currency: {port.Currency.ToString("C")}", port.Position.X, port.Position.Y, 20, Raylib.BLACK);
                }
            });
        }
    }
}
