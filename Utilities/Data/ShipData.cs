using System.Text.Json;

namespace NovemberPirates.Utilities.Data
{
    internal class ShipData
    {
        internal static ShipData Instance = new ShipData();

        private static int Version = 1;
        private string SavePath = "Assets/Data/GameData.json";

        private ShipData()
        {
            ImportOrCreateShipData();
        }
        private void ImportOrCreateShipData()
        {
            if (File.Exists(SavePath))
            {
                var dataString = File.ReadAllText(SavePath);
                var container = JsonSerializer.Deserialize<DataContainer>(dataString);
                if (container != null)
                {
                    Data = container.Data;
                }
            }
            else
            {
                SaveToJson();
            }
        }

        internal void SaveToJson()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            var container = new DataContainer
            {
                Version = Version,
                Data = Data
            };
            var dataString = JsonSerializer.Serialize(container, typeof(DataContainer), new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(SavePath, dataString);
        }

        internal Dictionary<string, float> Data = new()
        {
            // Small Ships
            { $"{BoatType.HullSmall}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullSmall}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullSmall}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullSmall}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullSmall}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullSmall}{ShipAbilities.TwoCannon}", 7 },
            { $"{BoatType.HullSmall}{ShipAbilities.MaxCrew}", 15 },

            { $"{BoatType.HullSmall}{Stats.RowingSpeed}", 100},
            { $"{BoatType.HullSmall}{Stats.TurningSpeed}", 100},
            { $"{BoatType.HullSmall}{Stats.MaxSpeed}", 500},
            { $"{BoatType.HullSmall}{Stats.HalfSailSpeed}", 1},
            { $"{BoatType.HullSmall}{Stats.FullSailSpeed}", 1},
            { $"{BoatType.HullSmall}{Stats.SailSpeedEasing}" , 1},
            { $"{BoatType.HullSmall}{Stats.InitialCrew}" , 10},
            { $"{BoatType.HullSmall}{Stats.InitialHp}" , 100},


            // Medium Ships
            { $"{BoatType.HullMedium}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullMedium}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullMedium}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullMedium}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullMedium}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullMedium}{ShipAbilities.TwoCannon}", 7 },
            { $"{BoatType.HullMedium}{ShipAbilities.MaxCrew}", 20 },

            { $"{BoatType.HullMedium}{Stats.RowingSpeed}", 100},
            { $"{BoatType.HullMedium}{Stats.TurningSpeed}", 100},
            { $"{BoatType.HullMedium}{Stats.MaxSpeed}", 500},
            { $"{BoatType.HullMedium}{Stats.HalfSailSpeed}", 1},
            { $"{BoatType.HullMedium}{Stats.FullSailSpeed}", 1},
            { $"{BoatType.HullMedium}{Stats.SailSpeedEasing}" , 1},
            { $"{BoatType.HullMedium}{Stats.InitialCrew}" , 10},
            { $"{BoatType.HullMedium}{Stats.InitialHp}" , 100},

            // Large Ships
            { $"{BoatType.HullLarge}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullLarge}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullLarge}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullLarge}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullLarge}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullLarge}{ShipAbilities.TwoCannon}", 7 },
            { $"{BoatType.HullLarge}{ShipAbilities.MaxCrew}", 25 },

            { $"{BoatType.HullLarge}{Stats.RowingSpeed}", 100},
            { $"{BoatType.HullLarge}{Stats.TurningSpeed}", 100},
            { $"{BoatType.HullLarge}{Stats.MaxSpeed}", 500},
            { $"{BoatType.HullLarge}{Stats.HalfSailSpeed}", 1},
            { $"{BoatType.HullLarge}{Stats.FullSailSpeed}", 1},
            { $"{BoatType.HullLarge}{Stats.SailSpeedEasing}" , 1},
            { $"{BoatType.HullLarge}{Stats.InitialCrew}" , 10},
            { $"{BoatType.HullLarge}{Stats.InitialHp}" , 100},


            // Cannon types
            { $"{CannonType.TrustyRusty}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.TrustyRusty}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.TrustyRusty}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonCrewDamage}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonSailDamage}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonSpread}", 100 },
            { $"{CannonType.TrustyRusty}{Stats.CannonballSpeed}", 1000 },
            { $"{CannonType.TrustyRusty}{Stats.CannonballDuration}", 0.75f },
            { $"{CannonType.TrustyRusty}{Stats.CannonsShotPer}", 1 },

            { $"{CannonType.PvtPepper}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.PvtPepper}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.PvtPepper}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonCrewDamage}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonSailDamage}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonSpread}", 100 },
            { $"{CannonType.PvtPepper}{Stats.CannonballSpeed}", 1000 },
            { $"{CannonType.PvtPepper}{Stats.CannonballDuration}", 0.75f },
            { $"{CannonType.PvtPepper}{Stats.CannonsShotPer}", 1 },

            { $"{CannonType.BFC1700}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.BFC1700}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.BFC1700}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.BFC1700}{Stats.CannonCrewDamage}", 5 },
            { $"{CannonType.BFC1700}{Stats.CannonSailDamage}", 5 },
            { $"{CannonType.BFC1700}{Stats.CannonSpread}", 100 },
            { $"{CannonType.BFC1700}{Stats.CannonballSpeed}", 1000 },
            { $"{CannonType.BFC1700}{Stats.CannonballDuration}", 0.75f },
            { $"{CannonType.BFC1700}{Stats.CannonsShotPer}", 1 },
        };
    }

    internal class DataContainer
    {
        public int Version { get; set; }
        public Dictionary<string, float> Data { get; set; } = new();
    }
    internal enum ShipAbilities
    {
        Steering,
        Rowing,
        HalfSail,
        FullSail,

        OneCannon,
        TwoCannon,
        ThreeCannon,
        FourCannon,
        FiveCannon,
        SixCannon,
        MaxCrew,
    }

    internal enum CannonType
    {
        TrustyRusty,
        PvtPepper,
        BFC1700,
    }

    internal enum Stats
    {
        // Sailing
        RowingSpeed,
        TurningSpeed,
        MaxSpeed,
        HalfSailSpeed,
        FullSailSpeed,
        SailSpeedEasing,

        // Cannons
        CannonReloadTime,
        CannonReloadRate,
        CannonHullDamage,
        CannonCrewDamage,
        CannonSailDamage,
        CannonSpread,
        CannonballSpeed,
        CannonballDuration,
        CannonsShotPer,

        // Ship
        InitialCrew,
        InitialHp,
    }
}
