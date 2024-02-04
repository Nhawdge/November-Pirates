using NovemberPirates.Utilities;
using NovemberPirates.Utilities.Maps;

namespace NovemberPirates.Components
{
    internal class Singleton
    {
        public DebugLevel Debug = DebugLevel.None;
        public string DebugText = "";

        internal Map Map;

        public AudioKey Music;
        internal bool InventoryOpen;
    }

    public enum DebugLevel
    {
        None,
        Low,
        Medium,
        High,
    }
}

