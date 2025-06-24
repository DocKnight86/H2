using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class SBInnKeeper : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override IShopSellInfo SellInfo => m_SellInfo;
        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Ale, 7, 20, 0x99F, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Wine, 7, 20, 0x9C7, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Liquor, 7, 20, 0x99B, 0));
               
                Add(new GenericBuyInfo(typeof(BreadLoaf), 6, 10, 0x103B, 0, true));
                Add(new GenericBuyInfo(typeof(CheeseWheel), 21, 10, 0x97E, 0, true));
                Add(new GenericBuyInfo(typeof(CookedBird), 17, 20, 0x9B7, 0, true));
                Add(new GenericBuyInfo(typeof(LambLeg), 8, 20, 0x160A, 0, true));
                Add(new GenericBuyInfo(typeof(ChickenLeg), 5, 20, 0x1608, 0, true));
                Add(new GenericBuyInfo(typeof(Ribs), 7, 20, 0x9F2, 0, true));

                Add(new GenericBuyInfo(typeof(Peach), 3, 20, 0x9D2, 0, true));
                Add(new GenericBuyInfo(typeof(Pear), 3, 20, 0x994, 0, true));
                Add(new GenericBuyInfo(typeof(Grapes), 3, 20, 0x9D1, 0, true));
                Add(new GenericBuyInfo(typeof(Apple), 3, 20, 0x9D0, 0, true));
                Add(new GenericBuyInfo(typeof(Banana), 2, 20, 0x171F, 0, true));
             
                Add(new GenericBuyInfo(typeof(Candle), 6, 20, 0xA28, 0, true));
                
                Add(new GenericBuyInfo(typeof(Dices), 2, 20, 0xFA7, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Dices), 1);
            }
        }
    }
}
