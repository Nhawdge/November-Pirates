using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
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
                port.Currency += (port.Population / 10_000) * Raylib.GetFrameTime();
                port.Population += (port.Population / 1000) * Raylib.GetFrameTime();
                port.Population = Math.Min(port.Population, 10_000);

                if (singleton.Debug >= DebugLevel.Low)
                {
                    Raylib.DrawText($"Port Currency: {port.Currency.ToString("C")}\nPop: {port.Population.ToString("0")}", port.Position.X, port.Position.Y, 20, Raylib.BLACK);
                }
            });
        }
    }
}
