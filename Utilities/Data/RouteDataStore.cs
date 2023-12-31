using NovemberPirates.Components;
using System.Numerics;

namespace NovemberPirates.Utilities.Data
{
    internal class RouteDataStore
    {
        internal static RouteDataStore Instance = new RouteDataStore();

        internal Dictionary<string, List<Route>> Routes = new();
        private RouteDataStore() { }

    }
    internal class Route
    {
        internal Port FromPort { get; set; }
        internal Port ToPort { get; set; }

        internal List<Vector2> RoutePoints = new();
    }
}