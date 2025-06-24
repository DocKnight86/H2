namespace Server.Items
{
    [Flipable(0x13db, 0x13e2)]
    public class StuddedChest : BaseArmor
    {
        [Constructable]
        public StuddedChest()
            : base(0x13DB)
        {
            Weight = 8.0;
        }

        public StuddedChest(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 50;
        public override int InitMaxHits => 75;
        public override int BasePhysicalResistance => Quality == ItemQuality.Exceptional ? 4 : 3;

        public override int StrReq => 35;

        public override ArmorMaterialType MaterialType => ArmorMaterialType.Studded;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.Half;

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
