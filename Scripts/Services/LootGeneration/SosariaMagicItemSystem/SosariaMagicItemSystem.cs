namespace Server.Items
{
    public class SosariaMagicItemSystem
    {
        private static readonly SkillName[] _PossibleSkills =
        [
            SkillName.Swords,
            SkillName.Fencing,
            SkillName.Macing,
            SkillName.Archery,
            SkillName.Wrestling,
            SkillName.Parry,
            SkillName.Tactics,
            SkillName.Anatomy,
            SkillName.Healing,
            SkillName.Magery,
            SkillName.Meditation,
            SkillName.EvalInt,
            SkillName.MagicResist,
            SkillName.AnimalTaming,
            SkillName.AnimalLore,
            SkillName.Veterinary,
            SkillName.Musicianship,
            SkillName.Provocation,
            SkillName.Discordance,
            SkillName.Peacemaking,
            SkillName.Stealing,
            SkillName.Stealth,
            SkillName.SpiritSpeak
        ];

        public static Item SosariaMagicClothing(int itemPower)
        {
            BaseClothing item = Loot.RandomClothing();
            item.Hue = Utility.RandomDyedHue();

            switch (itemPower)
            {
                // 1 = LESSER
                case 1:
                {
                    item.ItemPower = ItemPower.Lesser;

                    switch (Utility.Random(3))
                    {
                        case 0:
                        {
                            item.Attributes.BonusHits = Utility.Random(1, 3);
                            break;
                        }
                        case 1:
                        {
                            item.Attributes.BonusStam = Utility.Random(1, 3);
                            break;
                        }
                        case 2:
                        {
                            item.Attributes.BonusMana = Utility.Random(1, 3);
                            break;
                        }
                    }
                    
                    if (0.05 >= Utility.RandomDouble())
                    {
                        switch (Utility.Random(3))
                        {
                            case 0:
                            {
                                item.Attributes.RegenHits = Utility.Random(1, 2);
                                break;
                            }
                            case 1:
                            {
                                item.Attributes.RegenStam = Utility.Random(1, 2);
                                break;
                            }
                            case 2:
                            {
                                item.Attributes.RegenMana = Utility.Random(1, 2);
                                break;
                            }
                        }
                    }

                    if (0.01 >= Utility.RandomDouble())
                    {
                        switch (Utility.Random(2))
                        {
                            case 0:
                            {
                                item.Attributes.EnhancePotions = Utility.Random(1, 3);
                                break;
                            }
                            case 1:
                            {
                                item.SkillBonuses.SetValues(0, _PossibleSkills[Utility.Random(_PossibleSkills.Length)], Utility.Random(1, 2));
                                break;
                            }
                        }
                    }

                    break;
                }
                // 2 = GREATER
                case 2:
                {
                    item.ItemPower = ItemPower.Greater;

                    item.Attributes.BonusHits = Utility.Random(2, 4);

                    break;
                }
                // 3 = MAJOR
                case 3:
                {
                    item.ItemPower = ItemPower.Major;

                    item.Attributes.BonusHits = Utility.Random(3, 5);

                    break;
                }
                // 4 = LEGENDARY ARTIFACT
                case 4:
                {
                    item.ItemPower = ItemPower.LegendaryArtifact;

                    item.Attributes.BonusHits = Utility.Random(4, 6);

                    break;
                }
            }

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
                        DropItem(SosariaMagicClothing(1));
                        break;
                    case 1:
                        DropItem(SosariaMagicClothing(1));
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
