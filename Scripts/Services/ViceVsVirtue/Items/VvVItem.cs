namespace Server.Items
{
    public interface IVvVItem
    {
        bool IsVvVItem { get; set; }
    }

    public static class VvVEquipment
    {
        public static void CheckProperties(Item item)
        {
            if (item is PrimerOnArmsTalisman talisman && talisman.Attributes.AttackChance != 10)
            {
                talisman.Attributes.AttackChance = 10;
            }

            if (item is CrystallineRing ring && ring.Attributes.CastRecovery != 3)
            {
                ring.Attributes.CastRecovery = 3;
            }

            if (item is HuntersHeaddress hunters)
            {
                if (hunters.Resistances.Physical != 8)
                    hunters.Resistances.Physical = 8;

                if (hunters.Resistances.Fire != 4)
                    hunters.Resistances.Fire = 4;

                if (hunters.Resistances.Cold != -8)
                    hunters.Resistances.Cold = -8;

                if (hunters.Resistances.Poison != 9)
                    hunters.Resistances.Poison = 9;

                if (hunters.Resistances.Energy != 3)
                    hunters.Resistances.Energy = 3;
            }

            if (item is KasaOfTheRajin kasa && kasa.Attributes.DefendChance != 10)
            {
                kasa.Attributes.DefendChance = 10;
            }

            if (item is OrnamentOfTheMagician magician && magician.Attributes.RegenMana != 3)
            {
                magician.Attributes.RegenMana = 3;
            }

            if (item is RingOfTheVile vile && vile.Attributes.AttackChance != 25)
            {
                vile.Attributes.AttackChance = 25;
            }

            if (item is SpiritOfTheTotem totem)
            {
                if (totem.Resistances.Fire != 7)
                    totem.Resistances.Fire = 7;

                if (totem.Resistances.Cold != 2)
                    totem.Resistances.Cold = 2;

                if (totem.Resistances.Poison != 6)
                    totem.Resistances.Poison = 6;

                if (totem.Resistances.Energy != 6)
                    totem.Resistances.Energy = 6;
            }

            if (item is TomeOfLostKnowledge knowledge && knowledge.Attributes.RegenMana != 3)
            {
                knowledge.Attributes.RegenMana = 3;
            }
        }
    }
}
