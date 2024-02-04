namespace NovemberPirates.Scenes.Levels.DataManagers
{
    internal class InventoryManager
    {
        private InventoryManager() { }

        public static InventoryManager Instance = new();

        public long Gold = 0;
        public long Food = 0;
        public long Lumber = 0;

    }
}
