namespace Server.Items
{
    [Flipable(0x13da, 0x13e1)]
    public class StuddedLegs : BaseArmor
    {
        [Constructable]
        public StuddedLegs()
            : base(0x13DA)
        {
            Weight = 5.0;
        }

        public StuddedLegs(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 50;
        public override int InitMaxHits => 75;
        public override int BasePhysicalResistance => Quality == ItemQuality.Exceptional ? 4 : 3;

        public override int StrReq => 30;

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
