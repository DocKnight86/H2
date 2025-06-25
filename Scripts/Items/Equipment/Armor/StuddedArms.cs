namespace Server.Items
{
    [Flipable(0x13dc, 0x13d4)]
    public class StuddedArms : BaseArmor
    {
        [Constructable]
        public StuddedArms()
            : base(0x13DC)
        {
            Weight = 4.0;
        }

        public StuddedArms(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 50;
        public override int InitMaxHits => 75;
        public override int BasePhysicalResistance => Quality == ItemQuality.Exceptional ? 4 : 3;

        public override int StrReq => 25;

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
