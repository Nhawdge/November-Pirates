using Raylib_CsLo;

namespace NovemberPirates.Utilities
{
    internal class AudioManager
    {
        private AudioManager() { }
        public static AudioManager Instance = new();

        public Dictionary<AudioKey, Sound[]> AudioStore = new();

        internal void LoadAllAudio()
        {
            AudioStore.Add(AudioKey.CannonFire, new[] {
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_03.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_04.ogg"),
            });
            AudioStore.Add(AudioKey.CannonHitWater, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_CannonHitWater.ogg")
            });
            AudioStore.Add(AudioKey.CrewHitWater, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_CrewHitWater.ogg")
            });
            AudioStore.Add(AudioKey.ShipSailingWater, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_ShipSailingWater.ogg")
            });
            AudioStore.Add(AudioKey.Wind, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_Wind.ogg")
            });
        }
        internal Sound GetSound(AudioKey key)
        {
            if (AudioStore.Count <= 0)
            {
                LoadAllAudio();
            }
            var group = AudioStore[key];
            return group[Random.Shared.Next(0, group.Length)];
        }
    }
    internal enum AudioKey
    {
        CannonFire,
        ShipSailingWater,
        Wind,
        CannonHitWater,
        CrewHitWater,
    }
}
