using Raylib_CsLo;

namespace NovemberPirates.Utilities
{
    internal class AudioManager
    {
        private AudioManager() { }
        public static AudioManager Instance = new();

        public Dictionary<AudioKey, Sound[]> AudioStore = new();
        public Dictionary<AudioKey, Music> MusicStore = new();


        internal void LoadAllAudio()
        {
            AudioStore.Add(AudioKey.CannonFire, new[] {
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_03.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CannonFire_04.ogg"),
            });
            AudioStore.Add(AudioKey.CannonHitShip, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_ShipHit_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_ShipHit_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_ShipHit_03.ogg"),
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
            AudioStore.Add(AudioKey.CollectWood, new[] { // TODO 
                Raylib.LoadSound("Assets/Audio/sfx_CollectWood_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CollectWood_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_CollectWood_03.ogg"),
            });
            AudioStore.Add(AudioKey.SailOpen, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_SailOpen_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_SailOpen_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_SailOpen_03.ogg"),
            });
            AudioStore.Add(AudioKey.SailClose, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_SailClose_01.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_SailClose_02.ogg"),
                Raylib.LoadSound("Assets/Audio/sfx_SailClose_03.ogg"),
            });
            AudioStore.Add(AudioKey.WindInSail, new[]
            {
                Raylib.LoadSound("Assets/Audio/sfx_WindinSail.ogg"),
            });

            MusicStore.Add(AudioKey.Charge, Raylib.LoadMusicStream("Assets/Audio/Charge.wav"));
            MusicStore.Add(AudioKey.Drifting, Raylib.LoadMusicStream("Assets/Audio/Drifting.wav"));
            MusicStore.Add(AudioKey.TheBleedingOcean, Raylib.LoadMusicStream("Assets/Audio/The-Bleeding-Ocean.wav"));
            MusicStore.Add(AudioKey.DreamingOfTreasure, Raylib.LoadMusicStream("Assets/Audio/Dreaming-of-Treasure.wav"));
            MusicStore.Add(AudioKey.TheWarriorsOfTheWater, Raylib.LoadMusicStream("Assets/Audio/The-Warriors-of-the-Water.wav"));
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

        internal Music GetMusic(AudioKey key)
        {
            if (AudioStore.Count <= 0)
            {
                LoadAllAudio();
            }
            return MusicStore[key];
        }

        internal void StopAllSounds()
        {
            AudioStore.ToList().ForEach(x =>
            {
                x.Value.ToList().ForEach(s => Raylib.StopSound(s));
            });

            MusicStore.ToList().ForEach(x =>
            {
                Raylib.StopMusicStream(x.Value);
            });
        }
    }
    internal enum AudioKey
    {
        CannonFire,
        ShipSailingWater,
        Wind,
        CannonHitWater,
        CrewHitWater,
        CollectWood,
        SailClose,
        SailOpen,
        WindInSail,
        CannonHitShip,
        DreamingOfTreasure,
        Charge,
        Drifting,
        TheWarriorsOfTheWater,
        TheBleedingOcean,
    }
}
