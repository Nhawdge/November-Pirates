using NovemberPirates.Components;

namespace NovemberPirates.Utilities.Data
{
    internal static class ShipData
    {
        internal static bool CanDo(this Ship ship, ShipAbilities ability)
        {
            return ship.Crew >= Data[$"{ship.BoatType}{ability}"];
        }

        static readonly Dictionary<string, int> Data = new()
        {
            // Small Ships
            { $"{BoatType.HullSmall}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullSmall}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullSmall}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullSmall}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullSmall}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullSmall}{ShipAbilities.TwoCannon}", 7 },

            // Medium Ships
            { $"{BoatType.HullMedium}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullMedium}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullMedium}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullMedium}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullMedium}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullMedium}{ShipAbilities.TwoCannon}", 7 },

            // Large Ships
            { $"{BoatType.HullLarge}{ShipAbilities.Steering}", 1 },
            { $"{BoatType.HullLarge}{ShipAbilities.Rowing}",   2 },
            { $"{BoatType.HullLarge}{ShipAbilities.HalfSail}", 3 },
            { $"{BoatType.HullLarge}{ShipAbilities.FullSail}", 10 },
            { $"{BoatType.HullLarge}{ShipAbilities.OneCannon}", 5 },
            { $"{BoatType.HullLarge}{ShipAbilities.TwoCannon}", 7 },
        };
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
}
