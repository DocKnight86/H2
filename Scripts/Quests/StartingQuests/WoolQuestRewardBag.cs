namespace Server.Items
{
    public class WoolQuestRewardBag : BaseRewardBag
    {
        public static Item Clothing()
        {
            BaseClothing item = Loot.RandomClothing();

            item.Attributes.BonusHits = 1;

            return item;
        }

        [Constructable]
        public WoolQuestRewardBag()
        {
            switch (Utility.Random(2))
            {
                case 0:
                    DropItem(Clothing());
                    break;
                case 1:
                    DropItem(Clothing());
                    break;
            }
        }

        public WoolQuestRewardBag(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
