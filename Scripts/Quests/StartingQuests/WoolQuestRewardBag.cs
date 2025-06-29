namespace Server.Items
{
    public class WoolQuestRewardBag : Bag
    {
        [Constructable]
        public WoolQuestRewardBag()
        {
            DropItem(Loot.MagicClothing(1));
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
            reader.ReadInt();
        }
    }
}
