namespace Server.Items
{
    [Flipable(0x13cd, 0x13c5)]
    public class LeatherArms : BaseArmor
    {
        [Constructable]
        public LeatherArms()
            : base(0x13CD)
        {
            Weight = 2.0;
        }

        public LeatherArms(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits => 50;
        public override int InitMaxHits => 75;
        public override int BasePhysicalResistance => Quality == ItemQuality.Exceptional ? 3 : 2;

        public override int StrReq => 20;

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
