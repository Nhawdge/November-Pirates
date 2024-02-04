using NovemberPirates.Utilities.ContentData;

namespace NovemberPirates.Scenes.Levels.DataManagers
{
    internal class InventoryManager
    {
        private InventoryManager() { }

        public static InventoryManager Instance = new();

        public long Gold = 0;

        public List<TradeableGood> Inventory = new();

    }
}
