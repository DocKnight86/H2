namespace Server.Items
{
    public class SosariaMagicItemSystem
    {
        public static Item LesserMagicClothing()
        {
            BaseClothing item = Loot.RandomClothing();

            item.ItemPower = ItemPower.Lesser;

            return item;
        }

        public static Item GreaterMagicClothing()
        {
            BaseClothing item = Loot.RandomClothing();

            item.ItemPower = ItemPower.Greater;

            return item;
        }

        public static Item MajorMagicClothing()
        {
            BaseClothing item = Loot.RandomClothing();

            item.ItemPower = ItemPower.Major;

            return item;
        }

        public static Item LegendaryArtifactMagicClothing()
        {
            BaseClothing item = Loot.RandomClothing();

            item.ItemPower = ItemPower.LegendaryArtifact;

            return item;
        }

        public class TestBag : Bag
        {
            [Constructable]
            public TestBag()
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        DropItem(LesserMagicClothing());
                        break;
                    case 1:
                        DropItem(LesserMagicClothing());
                        break;
                }
            }

            public TestBag(Serial serial)
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
}
