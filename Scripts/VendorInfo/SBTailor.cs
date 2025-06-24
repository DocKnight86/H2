using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBTailor : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(SewingKit), 3, 20, 0xF9D, 0));
                Add(new GenericBuyInfo(typeof(Scissors), 11, 20, 0xF9F, 0));

                Add(new GenericBuyInfo(typeof(Shirt), 12, 20, 0x1517, 0));
                Add(new GenericBuyInfo(typeof(ShortPants), 7, 20, 0x152E, 0));
                Add(new GenericBuyInfo(typeof(FancyShirt), 21, 20, 0x1EFD, 0));
                Add(new GenericBuyInfo(typeof(LongPants), 10, 20, 0x1539, 0));
                Add(new GenericBuyInfo(typeof(FancyDress), 26, 20, 0x1EFF, 0));
                Add(new GenericBuyInfo(typeof(PlainDress), 13, 20, 0x1F01, 0));
                Add(new GenericBuyInfo(typeof(Kilt), 11, 20, 0x1537, 0));
                Add(new GenericBuyInfo(typeof(HalfApron), 10, 20, 0x153b, 0));
                Add(new GenericBuyInfo(typeof(Robe), 18, 20, 0x1F03, 0));
                Add(new GenericBuyInfo(typeof(Cloak), 8, 20, 0x1515, 0));
                Add(new GenericBuyInfo(typeof(Doublet), 13, 20, 0x1F7B, 0));
                Add(new GenericBuyInfo(typeof(Tunic), 18, 20, 0x1FA1, 0));

                Add(new GenericBuyInfo(typeof(FloppyHat), 7, 20, 0x1713, 0));
                Add(new GenericBuyInfo(typeof(WideBrimHat), 8, 20, 0x1714, 0));
                Add(new GenericBuyInfo(typeof(Cap), 10, 20, 0x1715, 0));
                Add(new GenericBuyInfo(typeof(TallStrawHat), 8, 20, 0x1716, 0));
                Add(new GenericBuyInfo(typeof(StrawHat), 7, 20, 0x1717, 0));
                Add(new GenericBuyInfo(typeof(FeatheredHat), 10, 20, 0x171A, 0));
                Add(new GenericBuyInfo(typeof(TricorneHat), 8, 20, 0x171B, 0));
                Add(new GenericBuyInfo(typeof(Bandana), 6, 20, 0x1540, 0));
                Add(new GenericBuyInfo(typeof(SkullCap), 7, 20, 0x1544, 0));

                Add(new GenericBuyInfo(typeof(BoltOfCloth), 100, 20, 0xf95, 0, true));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {

            }
        }
    }
}
