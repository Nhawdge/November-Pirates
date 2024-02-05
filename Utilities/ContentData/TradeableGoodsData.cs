using NovemberPirates.Utilities.Data;

namespace NovemberPirates.Utilities.ContentData
{
    internal class TradeableGoodsData
    {
        internal static TradeableGoodsData Instance = new();

        internal Dictionary<TradeableGoodsNames, TradeableGood> Goods = new();

        private TradeableGoodsData()
        {
            LoadPrices();
        }

        private void LoadPrices()
        {
            Goods.Add(TradeableGoodsNames.Lumber, new TradeableGood { Name = "Lumber", Price = 1 });
            Goods.Add(TradeableGoodsNames.Food, new TradeableGood { Name = "Food", Price = 5 });
            Goods.Add(TradeableGoodsNames.Material, new TradeableGood { Name = "Material", Price = 10 });
            Goods.Add(TradeableGoodsNames.Weapons, new TradeableGood { Name = "Weapons", Price = 20 });
            Goods.Add(TradeableGoodsNames.FineGoods, new TradeableGood { Name = "Fine Goods", Price = 40 });
            Goods.Add(TradeableGoodsNames.Gems, new TradeableGood { Name = "Gems", Price = 80 });

            //Cannon Prices
            Goods.Add(TradeableGoodsNames.TrustyRusty, new TradeableGood { Name = "Trusty Rusty", Price = 100, IsEquipable = true });
            Goods.Add(TradeableGoodsNames.PvtPepper, new TradeableGood { Name = "Pvt Pepper", Price = 200, IsEquipable = true });
            Goods.Add(TradeableGoodsNames.BFC1700, new TradeableGood { Name = "BFC 1700", Price = 400, IsEquipable = true });
        }

        static internal TradeableGood GetGood(TradeableGoodsNames name)
        {
            return Instance.Goods[name];
        }
    }

    internal class TradeableGood
    {
        internal string Name;
        internal int Price;
        internal bool IsEquipable = false;
        internal bool EquipModeActive;
    }

    internal enum TradeableGoodsNames
    {
        TrustyRusty = CannonType.TrustyRusty,
        PvtPepper = CannonType.PvtPepper,
        BFC1700 = CannonType.BFC1700,
        Lumber,
        Food,
        Material,
        Weapons,
        FineGoods,
        Gems,
    }

    internal enum UpgradeOptions
    {
        HullArmor, // HP Increase
        SailDurability, // Sail HP Increase
        RudderStrength, // Turn Speed
    }
}
