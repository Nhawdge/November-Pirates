using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class AudioSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var audioQuery = new QueryDescription().WithAll<AudioEvent>();
            var singleton = world.QueryFirst<Singleton>().Get<Singleton>();

            world.Query(in audioQuery, (entity) =>
            {
                var audioEvent = entity.Get<AudioEvent>();
                Raylib.PlaySound(AudioManager.Instance.GetSound(audioEvent.Key));
                world.Destroy(entity);

            });

        }
    }
}
