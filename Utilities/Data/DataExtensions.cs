using NovemberPirates.Components;

namespace NovemberPirates.Utilities.Data
{
    internal static class DataExtensions
    {
        internal static bool CanDo(this Ship ship, ShipAbilities ability)
        {
            return ship.Crew >= ShipData.Instance.Data[$"{ship.BoatType}{ability}"];
        }
        internal static float GetStat(this Ship ship, Stats stat)
        {
            return ShipData.Instance.Data[$"{ship.BoatType}{stat}"];
        }
    }
}
