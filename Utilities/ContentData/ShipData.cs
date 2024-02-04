using System.Text.Json;

namespace NovemberPirates.Utilities.Data
{
    internal class ShipData
    {
        internal static ShipData Instance = new ShipData();

        private int Version = 1;
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
                    if (container.Version == Version)
                    {
                        Data = container.Data;
                        return;
                    }
                }
            }
            SaveToJson();
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
            { $"{HullType.Small}{ShipAbilities.Steering}", 1 },
            { $"{HullType.Small}{ShipAbilities.Rowing}",   2 },
            { $"{HullType.Small}{ShipAbilities.HalfSail}", 3 },
            { $"{HullType.Small}{ShipAbilities.FullSail}", 10 },
            { $"{HullType.Small}{ShipAbilities.OneCannon}", 5 },
            { $"{HullType.Small}{ShipAbilities.TwoCannon}", 7 },

            { $"{HullType.Small}{Stats.RowingSpeed}", 100},
            { $"{HullType.Small}{Stats.TurningSpeed}", 100},
            { $"{HullType.Small}{Stats.MaxSpeed}", 500},
            { $"{HullType.Small}{Stats.HalfSailSpeed}", 1},
            { $"{HullType.Small}{Stats.FullSailSpeed}", 1},
            { $"{HullType.Small}{Stats.SailSpeedEasing}" , 1},
            { $"{HullType.Small}{Stats.InitialCrew}" , 10},
            { $"{HullType.Small}{Stats.MaxCrew}" , 15},
            { $"{HullType.Small}{Stats.HullHealth}" , 100},
            { $"{HullType.Small}{Stats.SailHealth}" , 100},


            // Medium Ships
            { $"{HullType.Medium}{ShipAbilities.Steering}", 1 },
            { $"{HullType.Medium}{ShipAbilities.Rowing}",   2 },
            { $"{HullType.Medium}{ShipAbilities.HalfSail}", 3 },
            { $"{HullType.Medium}{ShipAbilities.FullSail}", 10 },
            { $"{HullType.Medium}{ShipAbilities.OneCannon}", 5 },
            { $"{HullType.Medium}{ShipAbilities.TwoCannon}", 7 },
            

            { $"{HullType.Medium}{Stats.RowingSpeed}", 100},
            { $"{HullType.Medium}{Stats.TurningSpeed}", 100},
            { $"{HullType.Medium}{Stats.MaxSpeed}", 500},
            { $"{HullType.Medium}{Stats.HalfSailSpeed}", 1},
            { $"{HullType.Medium}{Stats.FullSailSpeed}", 1},
            { $"{HullType.Medium}{Stats.SailSpeedEasing}" , 1},
            { $"{HullType.Medium}{Stats.InitialCrew}" , 10},
            { $"{HullType.Medium}{Stats.MaxCrew}" , 20},
            { $"{HullType.Medium}{Stats.HullHealth}" , 100},
            { $"{HullType.Medium}{Stats.SailHealth}" , 100},

            // Large Ships
            { $"{HullType.Large}{ShipAbilities.Steering}", 1 },
            { $"{HullType.Large}{ShipAbilities.Rowing}",   2 },
            { $"{HullType.Large}{ShipAbilities.HalfSail}", 3 },
            { $"{HullType.Large}{ShipAbilities.FullSail}", 10 },
            { $"{HullType.Large}{ShipAbilities.OneCannon}", 5 },
            { $"{HullType.Large}{ShipAbilities.TwoCannon}", 7 },

            { $"{HullType.Large}{Stats.RowingSpeed}", 100},
            { $"{HullType.Large}{Stats.TurningSpeed}", 100},
            { $"{HullType.Large}{Stats.MaxSpeed}", 500},
            { $"{HullType.Large}{Stats.HalfSailSpeed}", 1},
            { $"{HullType.Large}{Stats.FullSailSpeed}", 1},
            { $"{HullType.Large}{Stats.SailSpeedEasing}" , 1},
            { $"{HullType.Large}{Stats.InitialCrew}" , 10},
            { $"{HullType.Large}{Stats.MaxCrew}" , 25},
            { $"{HullType.Large}{Stats.HullHealth}" , 100},
            { $"{HullType.Large}{Stats.SailHealth}" , 100},


            // Cannon types
            { $"{CannonType.TrustyRusty}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.TrustyRusty}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.TrustyRusty}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonCrewKillChance}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonCrewKillChainLimit}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonSailDamage}", 5 },
            { $"{CannonType.TrustyRusty}{Stats.CannonSpread}", 100 },
            { $"{CannonType.TrustyRusty}{Stats.CannonballSpeed}", 1000 },
            { $"{CannonType.TrustyRusty}{Stats.CannonballDuration}", 0.75f },
            { $"{CannonType.TrustyRusty}{Stats.CannonsShotPer}", 1 },

            { $"{CannonType.PvtPepper}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.PvtPepper}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.PvtPepper}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonCrewKillChance}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonCrewKillChainLimit}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonSailDamage}", 5 },
            { $"{CannonType.PvtPepper}{Stats.CannonSpread}", 100 },
            { $"{CannonType.PvtPepper}{Stats.CannonballSpeed}", 1000 },
            { $"{CannonType.PvtPepper}{Stats.CannonballDuration}", 0.75f },
            { $"{CannonType.PvtPepper}{Stats.CannonsShotPer}", 1 },

            { $"{CannonType.BFC1700}{Stats.CannonReloadTime}", 0.5f },
            { $"{CannonType.BFC1700}{Stats.CannonReloadRate}", 1 },
            { $"{CannonType.BFC1700}{Stats.CannonHullDamage}", 5 },
            { $"{CannonType.BFC1700}{Stats.CannonCrewKillChance}", 5 },
            { $"{CannonType.BFC1700}{Stats.CannonCrewKillChainLimit}", 5 },
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
        CannonSailDamage,
        CannonCrewKillChance,
        CannonCrewKillChainLimit,
        CannonSpread,
        CannonballSpeed,
        CannonballDuration,
        CannonsShotPer,

        // Ship
        InitialCrew,
        MaxCrew,
        HullHealth,
        SailHealth,
    }
}
