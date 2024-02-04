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
    }
    internal enum TradeableGoodsNames
    {
        Lumber,
        Food,
        Material,
        Weapons,
        FineGoods,
        Gems,
    }
}
