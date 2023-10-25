using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates;
using NovemberPirates.Components;
using NovemberPirates.Systems;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class CameraSystem : GameSystem
    {
        public CameraSystem()
        {
        }

        internal override void Update(World world)
        {
            var query = new QueryDescription().WithAll<Player>();

            world.Query(in query, (entity) =>
            {
                NovemberPiratesEngine.Instance.Camera.target.X = entity.Get<Sprite>().Position.X;
                NovemberPiratesEngine.Instance.Camera.target.Y = entity.Get<Sprite>().Position.Y;
            });
        }
    }
}
