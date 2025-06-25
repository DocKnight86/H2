namespace Server.Items
{
    [Flipable(0x13cc, 0x13d3)]
    public class LeatherChest : BaseArmor
    {
        [Constructable]
        public LeatherChest()
            : base(0x13CC)
        {
            Weight = 6.0;
        }

        public LeatherChest(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 50;
        public override int InitMaxHits => 75;
        public override int BasePhysicalResistance => Quality == ItemQuality.Exceptional ? 3 : 2;

        public override int StrReq => 25;

        public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
