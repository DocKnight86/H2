using Server.Items;
using System;

namespace Server
{
    public class Loot
    {
        public static Item DropMinorMagicClothing(IEntity e)
        {
            return MagicClothing(1);
        }

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

        public static Item MagicClothing(int itemPower)
        {
            BaseClothing item = Loot.RandomClothing();
            //item.Hue = Utility.RandomDyedHue();

            switch (itemPower)
            {
                // 1 = MINOR
                case 1:
                    {
                        item.ItemPower = ItemPower.Minor;

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
                // 2 = LESSER
                case 2:
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
                // 3 = GREATER
                case 3:
                    {
                        item.ItemPower = ItemPower.Greater;

                        item.Attributes.BonusHits = Utility.Random(2, 4);

                        break;
                    }
                // 4 = MAJOR
                case 4:
                    {
                        item.ItemPower = ItemPower.Major;

                        item.Attributes.BonusHits = Utility.Random(3, 5);

                        break;
                    }
                // 5 = LEGENDARY ARTIFACT
                case 5:
                    {
                        item.ItemPower = ItemPower.LegendaryArtifact;

                        item.Attributes.BonusHits = Utility.Random(4, 6);

                        break;
                    }
            }

            return item;
        }

        public static Item SosariaMagicWeapon(int itemPower)
        {
            BaseWeapon item = Loot.RandomWeapon();

            switch (itemPower)
            {
                // 1 = MINOR
                case 1:
                    {
                        item.ItemPower = ItemPower.Minor;

                        item.Attributes.WeaponDamage = 10;

                        switch (Utility.Random(3))
                        {
                            case 0:
                                {
                                    item.Attributes.AttackChance = Utility.Random(1, 3);
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
                // 2 = LESSER
                case 2:
                    {
                        item.ItemPower = ItemPower.Lesser;

                        break;
                    }
                // 3 = GREATER
                case 3:
                    {
                        item.ItemPower = ItemPower.Greater;

                        break;
                    }
                // 4 = MAJOR
                case 4:
                    {
                        item.ItemPower = ItemPower.Major;

                        break;
                    }
                // 5 = LEGENDARY ARTIFACT
                case 5:
                    {
                        item.ItemPower = ItemPower.LegendaryArtifact;

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
                DropItem(MagicClothing(1));
                DropItem(MagicClothing(1));
                DropItem(MagicClothing(1));
                DropItem(MagicClothing(1));
                DropItem(MagicClothing(1));
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








        #region List definitions

        #region Normal equipment
        private static readonly Type[] m_ClothingTypes =
        {
            typeof(Cloak), typeof(Bonnet), typeof(Cap), typeof(FeatheredHat), typeof(FloppyHat), typeof(JesterHat),
            typeof(Surcoat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(TricorneHat),
            typeof(WideBrimHat), typeof(Bandana),typeof(OrcMask), typeof(TribalMask), typeof(BearMask), typeof(DeerMask),
            typeof(WizardsHat), typeof(BodySash), typeof(Doublet), typeof(Boots), typeof(FullApron), typeof(JesterSuit),
            typeof(Sandals), typeof(Tunic), typeof(Shoes), typeof(Shirt), typeof(Kilt), typeof(Skirt), typeof(FancyShirt),
            typeof(FancyDress), typeof(ThighBoots), typeof(LongPants), typeof(PlainDress), typeof(Robe), typeof(ShortPants),
            typeof(HalfApron)
        };
        public static Type[] ClothingTypes => m_ClothingTypes;
		
		private static readonly Type[] m_WeaponTypes =
        {
            typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet),
            typeof(LargeBattleAxe), typeof(TwoHandedAxe), typeof(WarAxe), typeof(Club), typeof(Mace), typeof(Maul),
            typeof(WarHammer), typeof(WarMace), typeof(Bardiche), typeof(Halberd), typeof(Spear), typeof(ShortSpear),
            typeof(Pitchfork), typeof(WarFork), typeof(BlackStaff), typeof(GnarledStaff), typeof(QuarterStaff),
            typeof(Broadsword), typeof(Cutlass), typeof(Katana), typeof(Kryss), typeof(Longsword), typeof(Scimitar),
            typeof(VikingSword), typeof(Pickaxe), typeof(HammerPick), typeof(ButcherKnife), typeof(Cleaver), typeof(Dagger),
            typeof(SkinningKnife), typeof(ShepherdsCrook), typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow)
        };	
        public static Type[] WeaponTypes => m_WeaponTypes;

        private static readonly Type[] m_RangedWeaponTypes =
        { 
			typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow)
		};
        public static Type[] RangedWeaponTypes => m_RangedWeaponTypes;

        private static readonly Type[] m_ArmorTypes =
        {
            typeof(BoneArms), typeof(BoneChest), typeof(BoneGloves), typeof(BoneLegs), typeof(BoneHelm), typeof(ChainChest),
            typeof(ChainLegs), typeof(ChainCoif), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(NorseHelm),
            typeof(OrcHelm), typeof(FemaleLeatherChest), typeof(LeatherArms), typeof(LeatherBustierArms), typeof(LeatherChest),
            typeof(LeatherGloves), typeof(LeatherGorget), typeof(LeatherLegs), typeof(LeatherShorts), typeof(LeatherSkirt),
            typeof(LeatherCap), typeof(FemalePlateChest), typeof(PlateArms), typeof(PlateChest), typeof(PlateGloves),
            typeof(PlateGorget), typeof(PlateHelm), typeof(PlateLegs), typeof(RingmailArms), typeof(RingmailChest),
            typeof(RingmailGloves), typeof(RingmailLegs), typeof(FemaleStuddedChest), typeof(StuddedArms),
            typeof(StuddedBustierArms), typeof(StuddedChest), typeof(StuddedGloves), typeof(StuddedGorget), typeof(StuddedLegs)
        };
        public static Type[] ArmorTypes => m_ArmorTypes;
		
		private static readonly Type[] m_JewelryTypes =
        {
            typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet)
        };
        public static Type[] JewelryTypes => m_JewelryTypes;

        private static readonly Type[] m_ShieldTypes =
        {
            typeof(BronzeShield), typeof(Buckler), typeof(HeaterShield), typeof(MetalShield), typeof(MetalKiteShield),
            typeof(WoodenKiteShield), typeof(WoodenShield)
        };
        public static Type[] ShieldTypes => m_ShieldTypes;
		#endregion
		
        private static readonly Type[] m_GemTypes =
        {
            typeof(Amber), typeof(Amethyst), typeof(Citrine), typeof(Diamond), typeof(Emerald), typeof(Ruby), typeof(Sapphire),
            typeof(StarSapphire), typeof(Tourmaline)
        };
        public static Type[] GemTypes => m_GemTypes;

        private static readonly Type[] m_RareGemTypes =
        {
            typeof(BlueDiamond), typeof(DarkSapphire), typeof(EcruCitrine), typeof(FireRuby), typeof(PerfectEmerald), typeof(Turquoise), typeof(WhitePearl), typeof(BrilliantAmber)
        };
        public static Type[] RareGemTypes => m_RareGemTypes;

        private static readonly Type[] m_MLResources =
		{
            typeof(BlueDiamond), typeof(DarkSapphire), typeof(EcruCitrine), typeof(FireRuby), typeof(PerfectEmerald), typeof(Turquoise), typeof(WhitePearl), typeof(BrilliantAmber),
            typeof(LuminescentFungi), typeof(BarkFragment), typeof(SwitchItem), typeof(ParasiticPlant)
        };
        public static Type[] MLResources => m_MLResources;

        public static Type[] RegTypes => m_RegTypes;
        private static readonly Type[] m_RegTypes =
        {
            typeof(BlackPearl), typeof(Bloodmoss), typeof(Garlic), typeof(Ginseng), typeof(MandrakeRoot), typeof(Nightshade),
            typeof(SulfurousAsh), typeof(SpidersSilk)
        };

        private static readonly Type[] m_PotionTypes =
        {
            typeof(AgilityPotion), typeof(StrengthPotion), typeof(RefreshPotion), typeof(LesserCurePotion),
            typeof(LesserHealPotion), typeof(LesserPoisonPotion)
        };
        public static Type[] PotionTypes => m_PotionTypes;

        private static readonly Type[] m_InstrumentTypes = { typeof(Drums), typeof(Harp), typeof(LapHarp), typeof(Lute), typeof(Tambourine), typeof(TambourineTassel) };
        public static Type[] InstrumentTypes => m_InstrumentTypes;

        private static readonly Type[] m_StatueTypes =
        {
            typeof(StatueSouth), typeof(StatueSouth2), typeof(StatueNorth), typeof(StatueWest), typeof(StatueEast),
            typeof(StatueEast2), typeof(StatueSouthEast), typeof(BustSouth), typeof(BustEast)
        };

        public static Type[] StatueTypes => m_StatueTypes;

        #region Spell Scrolls
        private static readonly Type[] m_MageryScrollTypes =
        {
            typeof(ReactiveArmorScroll), typeof(ClumsyScroll), typeof(CreateFoodScroll), typeof(FeeblemindScroll),
            typeof(HealScroll), typeof(MagicArrowScroll), typeof(NightSightScroll), typeof(WeakenScroll), typeof(AgilityScroll),
            typeof(CunningScroll), typeof(CureScroll), typeof(HarmScroll), typeof(MagicTrapScroll), typeof(MagicUnTrapScroll),
            typeof(ProtectionScroll), typeof(StrengthScroll), typeof(BlessScroll), typeof(FireballScroll),
            typeof(MagicLockScroll), typeof(PoisonScroll), typeof(TelekinisisScroll), typeof(TeleportScroll),
            typeof(UnlockScroll), typeof(WallOfStoneScroll), typeof(ArchCureScroll), typeof(ArchProtectionScroll),
            typeof(CurseScroll), typeof(FireFieldScroll), typeof(GreaterHealScroll), typeof(LightningScroll),
            typeof(ManaDrainScroll), typeof(RecallScroll), typeof(BladeSpiritsScroll), typeof(DispelFieldScroll),
            typeof(IncognitoScroll), typeof(MagicReflectScroll), typeof(MindBlastScroll), typeof(ParalyzeScroll),
            typeof(PoisonFieldScroll), typeof(SummonCreatureScroll), typeof(DispelScroll), typeof(EnergyBoltScroll),
            typeof(ExplosionScroll), typeof(InvisibilityScroll), typeof(MarkScroll), typeof(MassCurseScroll),
            typeof(ParalyzeFieldScroll), typeof(RevealScroll), typeof(ChainLightningScroll), typeof(EnergyFieldScroll),
            typeof(FlamestrikeScroll), typeof(GateTravelScroll), typeof(ManaVampireScroll), typeof(MassDispelScroll),
            typeof(MeteorSwarmScroll), typeof(PolymorphScroll), typeof(EarthquakeScroll), typeof(EnergyVortexScroll),
            typeof(ResurrectionScroll), typeof(SummonAirElementalScroll), typeof(SummonDaemonScroll),
            typeof(SummonEarthElementalScroll), typeof(SummonFireElementalScroll), typeof(SummonWaterElementalScroll)
        };
        public static Type[] MageryScrollTypes => m_MageryScrollTypes;
        #endregion

        private static readonly Type[] m_WandTypes =
        {
            typeof(FireballWand), typeof(LightningWand), typeof(MagicArrowWand), typeof(GreaterHealWand), typeof(HarmWand),
            typeof(HealWand)
        };
        public static Type[] WandTypes => m_WandTypes;

        private static readonly Type[] m_LibraryBookTypes =
        {
            typeof(GrammarOfOrcish), typeof(CallToAnarchy), typeof(ArmsAndWeaponsPrimer), typeof(SongOfSamlethe),
            typeof(TaleOfThreeTribes), typeof(GuideToGuilds), typeof(BirdsOfBritannia), typeof(BritannianFlora),
            typeof(ChildrenTalesVol2), typeof(TalesOfVesperVol1), typeof(DeceitDungeonOfHorror), typeof(DimensionalTravel),
            typeof(EthicalHedonism), typeof(MyStory), typeof(DiversityOfOurLand), typeof(QuestOfVirtues), typeof(RegardingLlamas),
            typeof(TalkingToWisps), typeof(TamingDragons), typeof(BoldStranger), typeof(BurningOfTrinsic), typeof(TheFight),
            typeof(LifeOfATravellingMinstrel), typeof(MajorTradeAssociation), typeof(RankingsOfTrades),
            typeof(WildGirlOfTheForest), typeof(TreatiseOnAlchemy), typeof(VirtueBook)
        };
        public static Type[] LibraryBookTypes => m_LibraryBookTypes;
        #endregion

        #region Accessors
        public static BaseWand RandomWand()
        {
            return Construct(m_WandTypes) as BaseWand;
        }

        public static BaseClothing RandomClothing()
        {
            return Construct(m_ClothingTypes) as BaseClothing;
        }

        public static BaseWeapon RandomRangedWeapon()
        {
            return Construct(m_RangedWeaponTypes) as BaseWeapon;
        }

        public static BaseWeapon RandomWeapon()
        {
            return Construct(m_WeaponTypes) as BaseWeapon;
        }

        public static BaseJewel RandomJewelry()
        {
            return Construct(m_JewelryTypes) as BaseJewel;
        }

        public static BaseArmor RandomArmor()
        {
            return Construct(m_ArmorTypes) as BaseArmor;
        }

        public static BaseShield RandomShield()
        {
            return Construct(m_ShieldTypes) as BaseShield;
        }

        public static BaseArmor RandomArmorOrShield()
        {
            return Construct(m_ArmorTypes, m_ShieldTypes) as BaseArmor;
        }

        public static Item RandomArmorOrShieldOrWeaponOrJewelry()
        {
            return Construct(m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_ShieldTypes, m_JewelryTypes);
        }

        #region Chest of Heirlooms
        public static Item ChestOfHeirloomsContains()
        {
            return Construct(m_ArmorTypes, m_WeaponTypes, m_RangedWeaponTypes, m_JewelryTypes);
        }
        #endregion

        public static Item RandomGem()
        {
            return Construct(m_GemTypes);
        }

        public static Item RandomRareGem()
        {
            return Construct(m_RareGemTypes);
        }

        public static Item RandomMLResource()
        {
            return Construct(m_MLResources);
        }

        public static Item RandomReagent()
        {
            return Construct(m_RegTypes);
        }

        public static Item RandomPotion()
        {
            return Construct(m_PotionTypes);
        }

        public static BaseInstrument RandomInstrument()
        {
            return Construct(m_InstrumentTypes) as BaseInstrument;
        }

        public static Item RandomStatue()
        {
            return Construct(m_StatueTypes);
        }

        public static SpellScroll RandomScroll(int minIndex, int maxIndex)
        {
            return Construct(m_MageryScrollTypes, Utility.RandomMinMax(minIndex, maxIndex)) as SpellScroll;
        }

        public static BaseTalisman RandomTalisman()
        {
            return new RandomTalisman();
        }
        #endregion

        #region Construction methods
        public static Item Construct(Type type)
        {
            Item item;

            try
            {
                item = Activator.CreateInstance(type) as Item;
            }
            catch (Exception e)
            {
                Diagnostics.ExceptionLogging.LogException(e);
                return null;
            }

            return item;
        }

        public static Item Construct(Type[] types)
        {
            if (types.Length > 0)
            {
                return Construct(types, Utility.Random(types.Length));
            }

            return null;
        }

        public static Item Construct(Type[] types, int index)
        {
            if (index >= 0 && index < types.Length)
            {
                return Construct(types[index]);
            }

            return null;
        }

        public static Item Construct(params Type[][] types)
        {
            int totalLength = 0;

            for (int i = 0; i < types.Length; ++i)
            {
                totalLength += types[i].Length;
            }

            if (totalLength > 0)
            {
                int index = Utility.Random(totalLength);

                for (int i = 0; i < types.Length; ++i)
                {
                    if (index >= 0 && index < types[i].Length)
                    {
                        return Construct(types[i][index]);
                    }

                    index -= types[i].Length;
                }
            }

            return null;
        }
        #endregion
    }
}
