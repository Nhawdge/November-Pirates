using Arch.Core;
using Arch.Core.Extensions;
using NovemberPirates.Components;
using NovemberPirates.Extensions;
using NovemberPirates.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class AudioSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var audioQuery = new QueryDescription().WithAll<AudioEvent>();
            var singleton = world.QueryFirst<Singleton>().Get<Singleton>();

            var music = AudioManager.Instance.GetMusic(singleton.Music);
            if (Raylib.IsMusicStreamPlaying(music))
            {
                Raylib.UpdateMusicStream(music);
            }
            else
            {
                Raylib.PlayMusicStream(AudioManager.Instance.GetMusic(singleton.Music));
            }

            world.Query(in audioQuery, (entity) =>
            {
                var audioEvent = entity.Get<AudioEvent>();
                var sound = AudioManager.Instance.GetSound(audioEvent.Key);

                var playerEntity = world.QueryFirst<Player>();
                var playerSprite = playerEntity.Get<Sprite>();

                var distance = Vector2.Distance(playerSprite.Position, audioEvent.Position);
                var maxDistance = 1280f; // TODO Get dynamically later
                if (distance < maxDistance)
                {
                    var volume = 1 - (distance / maxDistance);
                    var range = maxDistance * 2;
                    var pan = ((audioEvent.Position.X - playerSprite.Position.X) + maxDistance) / range;

                    Raylib.SetSoundPan(sound, pan);

                    Raylib.SetSoundVolume(sound, volume);

                    if (audioEvent.AllowMultiple || !Raylib.IsSoundPlaying(sound))
                        Raylib.PlaySound(sound);
                }
                else if (audioEvent.Position == Vector2.Zero)
                {
                    if (audioEvent.AllowMultiple || !Raylib.IsSoundPlaying(sound))
                        Raylib.PlaySound(sound);
                }

                if (!audioEvent.Replay)
                    world.Destroy(entity);

            });

        }
    }
}
