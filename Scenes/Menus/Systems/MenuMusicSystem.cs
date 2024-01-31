using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Menus.Systems
{
    internal class MenuMusicSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var singletonEntity = world.QueryFirst<Singleton>();
            var singleton = singletonEntity.Get<Singleton>();

            var music = AudioManager.Instance.GetMusic(singleton.Music);
            if (Raylib.IsMusicStreamPlaying(music))
            {
                Raylib.UpdateMusicStream(music);
            }
            else
            {
                Raylib.PlayMusicStream(AudioManager.Instance.GetMusic(singleton.Music));
            }
        }
    }
}
