using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public enum GemType
    {
        None,
        StarSapphire,
        Emerald,
        Sapphire,
        Ruby,
        Citrine,
        Amethyst,
        Tourmaline,
        Amber,
        Diamond
    }

    public abstract class BaseJewel : Item, ICraftable, ISetItem, IWearableDurability, IResource, IVvVItem, IOwnerRestricted, ITalismanProtection, IArtifact, ICombatEquipment, IQuality
    {
        private int _MaxHitPoints;
        private int _HitPoints;

        private AosAttributes m_AosAttributes;
        private AosElementAttributes m_AosResistances;
        private AosSkillBonuses _AosSkillBonuses;
        private SAAbsorptionAttributes _SAAbsorptionAttributes;
        private NegativeAttributes _NegativeAttributes;
        private CraftResource _Resource;
        private GemType _GemType;

        #region Stygian Abyss
        private int _TimesImbued;
        private bool _IsImbued;
        private int _GorgonLenseCharges;
        private LenseType _GorgonLenseType;
        #endregion

        private ItemPower _ItemPower;
        private ReforgedPrefix _ReforgedPrefix;
        private ReforgedSuffix _ReforgedSuffix;

        private TalismanAttribute _TalismanProtection;

        private bool _VvVItem;
        private Mobile _Owner;
        private string _OwnerName;

        [CommandProperty(AccessLevel.GameMaster)]
        public TalismanAttribute Protection
        {
            get { return _TalismanProtection; }
            set { _TalismanProtection = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsVvVItem
        {
            get { return _VvVItem; }
            set { _VvVItem = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return _Owner; }
            set { _Owner = value; if (_Owner != null)
                {
                    _OwnerName = _Owner.Name;
                }

                InvalidateProperties(); }
        }

        public virtual string OwnerName
        {
            get { return _OwnerName; }
            set { _OwnerName = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get => _MaxHitPoints;
            set
            {
                _MaxHitPoints = value;

                if (_MaxHitPoints > 255)
                {
                    _MaxHitPoints = 255;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get => _HitPoints;
            set
            {
                if (value != _HitPoints && MaxHitPoints > 0)
                {
                    _HitPoints = value;

                    if (_HitPoints < 0)
                    {
                        Delete();
                    }
                    else if (_HitPoints > MaxHitPoints)
                    {
                        _HitPoints = MaxHitPoints;
                    }

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.Player)]
        public AosAttributes Attributes
        {
            get => m_AosAttributes;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosElementAttributes Resistances
        {
            get => m_AosResistances;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses
        {
            get => _AosSkillBonuses;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SAAbsorptionAttributes AbsorptionAttributes
        {
            get => _SAAbsorptionAttributes;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.Player)]
        public NegativeAttributes NegativeAttributes
        {
            get => _NegativeAttributes;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get => _Resource;
            set
            {
                _Resource = value;
                Hue = CraftResources.GetHue(_Resource);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public GemType GemType
        {
            get => _GemType;
            set
            {
                GemType old = _GemType;
                _GemType = value;
                OnGemTypeChange(old);
                InvalidateProperties();
            }
        }

        #region SA
        [CommandProperty(AccessLevel.GameMaster)]
        public int TimesImbued
        {
            get { return _TimesImbued; }
            set { _TimesImbued = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsImbued
        {
            get
            {
                if (TimesImbued >= 1 && !_IsImbued)
                {
                    _IsImbued = true;
                }

                return _IsImbued;
            }
            set
            {
                if (TimesImbued >= 1)
                {
                    _IsImbued = true;
                }
                else
                {
                    _IsImbued = value;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GorgonLenseCharges
        {
            get { return _GorgonLenseCharges; }
            set { _GorgonLenseCharges = value; if (value == 0) _GorgonLenseType = LenseType.None; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public LenseType GorgonLenseType
        {
            get { return _GorgonLenseType; }
            set { _GorgonLenseType = value; InvalidateProperties(); }
        }

        public virtual int[] BaseResists => new int[] { 0, 0, 0, 0, 0 };

        public virtual void OnAfterImbued(Mobile m, int mod, int value)
        {
        }
        #endregion

        #region Runic Reforging
        [CommandProperty(AccessLevel.GameMaster)]
        public ReforgedPrefix ReforgedPrefix
        {
            get { return _ReforgedPrefix; }
            set { _ReforgedPrefix = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ReforgedSuffix ReforgedSuffix
        {
            get { return _ReforgedSuffix; }
            set { _ReforgedSuffix = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemPower ItemPower
        {
            get { return _ItemPower; }
            set { _ItemPower = value; InvalidateProperties(); }
        }
        #endregion

        public override int PhysicalResistance => m_AosResistances.Physical;
        public override int FireResistance => m_AosResistances.Fire;
        public override int ColdResistance => m_AosResistances.Cold;
        public override int PoisonResistance => m_AosResistances.Poison;
        public override int EnergyResistance => m_AosResistances.Energy;
        public virtual int BaseGemTypeNumber => 0;

        public virtual int InitMinHits => 0;
        public virtual int InitMaxHits => 0;

        public override int LabelNumber
        {
            get
            {
                if (_GemType == GemType.None)
                {
                    return base.LabelNumber;
                }

                return BaseGemTypeNumber + (int)_GemType - 1;
            }
        }

        public override double DefaultWeight
        {
            get
            {
                if (NegativeAttributes == null || NegativeAttributes.Unwieldly == 0)
                {
                    return base.DefaultWeight;
                }

                return 50;
            }
        }

        public override void OnAfterDuped(Item newItem)
        {
            BaseJewel jewel = newItem as BaseJewel;

            if (jewel == null)
            {
                return;
            }

            jewel.m_AosAttributes = new AosAttributes(newItem, m_AosAttributes);
            jewel.m_AosResistances = new AosElementAttributes(newItem, m_AosResistances);
            jewel._AosSkillBonuses = new AosSkillBonuses(newItem, _AosSkillBonuses);
            jewel._NegativeAttributes = new NegativeAttributes(newItem, _NegativeAttributes);
            jewel._TalismanProtection = new TalismanAttribute(_TalismanProtection);
            jewel._SetAttributes = new AosAttributes(newItem, _SetAttributes);
            jewel._SetSkillBonuses = new AosSkillBonuses(newItem, _SetSkillBonuses);
            jewel._AosSkillBonuses = new AosSkillBonuses(newItem, _AosSkillBonuses);

            base.OnAfterDuped(newItem);
        }

        public virtual int ArtifactRarity => 0;

        public override bool DisplayWeight
        {
            get
            {
                if (IsVvVItem)
                {
                    return true;
                }

                return base.DisplayWeight;
            }
        }

        private Mobile _Crafter;
        private ItemQuality _Quality;
        private bool _PlayerConstructed;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get => _Crafter;
            set
            {
                _Crafter = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality
        {
            get => _Quality;
            set
            {
                _Quality = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get => _PlayerConstructed;
            set
            {
                _PlayerConstructed = value;
                InvalidateProperties();
            }
        }

        public BaseJewel(int itemID, Layer layer)
            : base(itemID)
        {
            m_AosAttributes = new AosAttributes(this);
            m_AosResistances = new AosElementAttributes(this);
            _AosSkillBonuses = new AosSkillBonuses(this);
            _Resource = CraftResource.Iron;
            _GemType = GemType.None;

            Layer = layer;

            _HitPoints = _MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

            _SetAttributes = new AosAttributes(this);
            _SetSkillBonuses = new AosSkillBonuses(this);
            _SAAbsorptionAttributes = new SAAbsorptionAttributes(this);
            _NegativeAttributes = new NegativeAttributes(this);
            _TalismanProtection = new TalismanAttribute();
        }

        #region Stygian Abyss
        public override bool CanEquip(Mobile from)
        {
            if (from.IsPlayer())
            {
                if (_Owner != null && _Owner != from)
                {
                    from.SendLocalizedMessage(501023); // You must be the owner to use this item.
                    return false;
                }

                if (this is IAccountRestricted && ((IAccountRestricted)this).Account != null)
                {
                    Accounting.Account acct = from.Account as Accounting.Account;

                    if (acct == null || acct.Username != ((IAccountRestricted)this).Account)
                    {
                        from.SendLocalizedMessage(1071296); // This item is Account Bound and your character is not bound to it. You cannot use this item.
                        return false;
                    }
                }

                if (IsVvVItem && !Engines.VvV.ViceVsVirtueSystem.IsVvV(from))
                {
                    from.SendLocalizedMessage(1155496); // This item can only be used by VvV participants!
                    return false;
                }
            }

            return base.CanEquip(from);
        }

        public virtual int OnHit(BaseWeapon weap, int damageTaken)
        {
            if (_TimesImbued == 0 && _MaxHitPoints == 0)
            {
                return damageTaken;
            }

            //Sanity check incase some one has a bad state Jewel.
            if (_TimesImbued >= 1 && _MaxHitPoints == 0)
            {
                return damageTaken;
            }

            double chance = NegativeAttributes.Antique > 0 ? 80 : 25;

            if (chance >= Utility.Random(100)) // 25% chance to lower durability
            {
                int wear = 1;

                if (wear > 0)
                {
                    if (_HitPoints >= wear)
                    {
                        HitPoints -= wear;
                        wear = 0;
                    }
                    else
                    {
                        wear -= HitPoints;
                        HitPoints = 0;
                    }

                    if (wear > 0)
                    {
                        if (_MaxHitPoints > wear)
                        {
                            MaxHitPoints -= wear;

                            if (Parent is Mobile mobile)
                            {
                                mobile.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                            }
                        }
                        else
                        {
                            Delete();
                        }
                    }
                }
            }

            return damageTaken;
        }

        public virtual void UnscaleDurability()
        {
        }

        public virtual void ScaleDurability()
        {
        }

        public virtual bool CanFortify => IsImbued == false && NegativeAttributes.Antique < 4;
        #endregion

        public override void OnAdded(object parent)
        {
            if (parent is Mobile from)
            {
                _AosSkillBonuses.AddTo(from);

                int strBonus = m_AosAttributes.BonusStr;
                int dexBonus = m_AosAttributes.BonusDex;
                int intBonus = m_AosAttributes.BonusInt;

                if (strBonus != 0 || dexBonus != 0 || intBonus != 0)
                {
                    string modName = Serial.ToString();

                    if (strBonus != 0)
                    {
                        from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));
                    }

                    if (dexBonus != 0)
                    {
                        from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));
                    }

                    if (intBonus != 0)
                    {
                        from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
                    }
                }

                from.CheckStatTimers();

                #region Mondain's Legacy Sets
                if (IsSetItem)
                {
                    _SetEquipped = SetHelper.FullSetEquipped(from, SetID, Pieces);

                    if (_SetEquipped)
                    {
                        _LastEquipped = true;
                        SetHelper.AddSetBonus(from, SetID);
                    }
                }
                #endregion
            }
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile from)
            {
                _AosSkillBonuses.Remove();

                string modName = Serial.ToString();

                from.RemoveStatMod(modName + "Str");
                from.RemoveStatMod(modName + "Dex");
                from.RemoveStatMod(modName + "Int");

                from.CheckStatTimers();

                if (IsSetItem && _SetEquipped)
                {
                    SetHelper.RemoveSetBonus(from, SetID, this);
                }
            }
        }

        public virtual void SetProtection(Type type, TextDefinition name, int amount)
        {
            _TalismanProtection = new TalismanAttribute(type, name, amount);
        }

        public BaseJewel(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (_ReforgedPrefix != ReforgedPrefix.None || _ReforgedSuffix != ReforgedSuffix.None)
            {
                if (_ReforgedPrefix != ReforgedPrefix.None)
                {
                    int prefix = RunicReforging.GetPrefixName(_ReforgedPrefix);

                    if (_ReforgedSuffix == ReforgedSuffix.None)
                    {
                        list.Add(1151757, $"#{prefix}\t{GetNameString()}"); // ~1_PREFIX~ ~2_ITEM~
                    }
                    else
                    {
                        list.Add(1151756, $"#{prefix}\t{GetNameString()}\t#{RunicReforging.GetSuffixName(_ReforgedSuffix)}"); // ~1_PREFIX~ ~2_ITEM~ of ~3_SUFFIX~
                    }
                }
                else if (_ReforgedSuffix != ReforgedSuffix.None)
                {
                    RunicReforging.AddSuffixName(list, _ReforgedSuffix, GetNameString());
                }
            }
            else
            {
                base.AddNameProperty(list);
            }
        }

        private string GetNameString()
        {
            string name = Name;

            if (name == null)
            {
                name = $"#{LabelNumber}";
            }

            return name;
        }

        public override void AddCraftedProperties(ObjectPropertyList list)
        {
            if (OwnerName != null)
            {
                list.Add(1153213, OwnerName);
            }

            if (_Crafter != null)
            {
                list.Add(1050043, _Crafter.TitleName); // crafted by ~1_NAME~
            }

            if (_Quality == ItemQuality.Exceptional)
            {
                list.Add(1063341); // exceptional
            }

            if (IsImbued)
            {
                list.Add(1080418); // (Imbued)
            }
        }

        public override void AddWeightProperty(ObjectPropertyList list)
        {
            base.AddWeightProperty(list);

            if (IsVvVItem)
            {
                list.Add(1154937); // VvV Item
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (_GorgonLenseCharges > 0)
            {
                list.Add(1112590, _GorgonLenseCharges.ToString()); //Gorgon Lens Charges: ~1_val~
            }

            #region Mondain's Legacy Sets
            if (IsSetItem)
            {
                list.Add(1080240, Pieces.ToString()); // Part of a Jewelry Set (~1_val~ pieces)

                if (SetID == SetItem.Bestial)
                {
                    list.Add(1151541, BestialSetHelper.GetTotalBerserk(this).ToString()); // Berserk ~1_VAL~
                }

                if (BardMasteryBonus)
                {
                    list.Add(1151553); // Activate: Bard Mastery Bonus x2<br>(Effect: 1 min. Cooldown: 30 min.)
                }

                if (_SetEquipped)
                {
                    list.Add(1080241); // Full Jewelry Set Present					
                    SetHelper.GetSetProperties(list, this);
                }
            }
            #endregion

            _NegativeAttributes.GetProperties(list, this);
            _AosSkillBonuses.GetProperties(list);

            int prop;

            if ((prop = ArtifactRarity) > 0)
            {
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~
            }

            if (_TalismanProtection != null && !_TalismanProtection.IsEmpty && _TalismanProtection.Amount > 0)
            {
                list.Add(1072387, "{0}\t{1}", _TalismanProtection.Name != null ? _TalismanProtection.Name.ToString() : "Unknown", _TalismanProtection.Amount); // ~1_NAME~ Protection: +~2_val~%
            }

            if ((prop = _SAAbsorptionAttributes.EaterKinetic) != 0)
            {
                list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%
            }

            if ((prop = _SAAbsorptionAttributes.EaterDamage) != 0)
            {
                list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%
            }

            if ((prop = _SAAbsorptionAttributes.CastingFocus) != 0)
            {
                list.Add(1113696, prop.ToString()); // Casting Focus ~1_val~%
            }

            if ((prop = m_AosAttributes.SpellChanneling) != 0)
            {
                list.Add(1060482); // spell channeling
            }

            if ((prop = m_AosAttributes.NightSight) != 0)
            {
                list.Add(1060441); // night sight
            }

            if ((prop = m_AosAttributes.BonusStr) != 0)
            {
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~
            }

            if ((prop = m_AosAttributes.BonusDex) != 0)
            {
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~
            }

            if ((prop = m_AosAttributes.BonusInt) != 0)
            {
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~
            }

            if ((prop = m_AosAttributes.BonusHits) != 0)
            {
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~
            }

            if ((prop = m_AosAttributes.BonusStam) != 0)
            {
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~
            }

            if ((prop = m_AosAttributes.BonusMana) != 0)
            {
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~
            }

            if ((prop = m_AosAttributes.RegenHits) != 0)
            {
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~
            }

            if ((prop = m_AosAttributes.RegenStam) != 0)
            {
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~
            }

            if ((prop = m_AosAttributes.RegenMana) != 0)
            {
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~
            }

            if ((prop = m_AosAttributes.EnhancePotions) != 0)
            {
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%
            }

            if ((prop = m_AosAttributes.ReflectPhysical) != 0)
            {
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%
            }

            if ((prop = m_AosAttributes.AttackChance) != 0)
            {
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%
            }

            if ((prop = m_AosAttributes.WeaponSpeed) != 0)
            {
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%
            }

            if ((prop = m_AosAttributes.WeaponDamage) != 0)
            {
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%
            }

            if ((prop = m_AosAttributes.DefendChance) != 0)
            {
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%
            }

            if ((prop = m_AosAttributes.CastRecovery) != 0)
            {
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~
            }

            if ((prop = m_AosAttributes.CastSpeed) != 0)
            {
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~
            }

            if ((prop = m_AosAttributes.SpellDamage) != 0)
            {
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%
            }

            if ((prop = m_AosAttributes.LowerManaCost) != 0)
            {
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%
            }

            if ((prop = m_AosAttributes.LowerRegCost) != 0)
            {
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%      
            }

            if ((prop = m_AosAttributes.IncreasedKarmaLoss) != 0)
            {
                list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%
            }

            base.AddResistanceProperties(list);

            if (_HitPoints >= 0 && _MaxHitPoints > 0)
            {
                list.Add(1060639, "{0}\t{1}", _HitPoints, _MaxHitPoints); // durability ~1_val~ / ~2_val~
            }

            if (IsSetItem && !_SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                SetHelper.GetSetProperties(list, this);
            }
        }

        public override void AddItemPowerProperties(ObjectPropertyList list)
        {
            if (_ItemPower != ItemPower.None)
            {
                if (_ItemPower <= ItemPower.LegendaryArtifact)
                {
                    list.Add(1151488 + ((int)_ItemPower - 1));
                }
                else
                {
                    list.Add(1152281 + ((int)_ItemPower - 9));
                }
            }
        }

        public virtual void OnGemTypeChange(GemType old)
        {
        }

        public int GemLocalization()
        {
            switch (_GemType)
            {
                default:
                case GemType.None: return 0;
                case GemType.StarSapphire: return 1023867;
                case GemType.Emerald: return 1023887;
                case GemType.Sapphire: return 1023887;
                case GemType.Ruby: return 1023868;
                case GemType.Citrine: return 1023875;
                case GemType.Amethyst: return 1023863;
                case GemType.Tourmaline: return 1023872;
                case GemType.Amber: return 1062607;
                case GemType.Diamond: return 1062608;
            }
        }

        public override bool DropToWorld(Mobile from, Point3D p)
        {
            bool drop = base.DropToWorld(from, p);

            EnchantedHotItemSocket.CheckDrop(from, this);

            return drop;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(_SetPhysicalBonus);
            writer.Write(_SetFireBonus);
            writer.Write(_SetColdBonus);
            writer.Write(_SetPoisonBonus);
            writer.Write(_SetEnergyBonus);

            writer.Write(_PlayerConstructed);

            _TalismanProtection.Serialize(writer);

            writer.Write(_Owner);
            writer.Write(_OwnerName);

            writer.Write(_IsImbued);

            _NegativeAttributes.Serialize(writer);

            writer.Write((int)_ReforgedPrefix);
            writer.Write((int)_ReforgedSuffix);
            writer.Write((int)_ItemPower);

            writer.Write(_GorgonLenseCharges);
            writer.Write((int)_GorgonLenseType);

            writer.WriteEncodedInt(_TimesImbued);

            _SAAbsorptionAttributes.Serialize(writer);

            writer.Write(_LastEquipped);
            writer.Write(_SetEquipped);
            writer.WriteEncodedInt(_SetHue);

            _SetAttributes.Serialize(writer);
            _SetSkillBonuses.Serialize(writer);

            writer.Write(_Crafter);
            writer.Write((int)_Quality);

            writer.WriteEncodedInt(_MaxHitPoints);
            writer.WriteEncodedInt(_HitPoints);

            writer.WriteEncodedInt((int)_Resource);
            writer.WriteEncodedInt((int)_GemType);

            m_AosAttributes.Serialize(writer);
            m_AosResistances.Serialize(writer);
            _AosSkillBonuses.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        _SetPhysicalBonus = reader.ReadInt();
                        _SetFireBonus = reader.ReadInt();
                        _SetColdBonus = reader.ReadInt();
                        _SetPoisonBonus = reader.ReadInt();
                        _SetEnergyBonus = reader.ReadInt();

                        _PlayerConstructed = reader.ReadBool();

                        _TalismanProtection = new TalismanAttribute(reader);

                        _Owner = reader.ReadMobile();
                        _OwnerName = reader.ReadString();

                        _IsImbued = reader.ReadBool();

                        _NegativeAttributes = new NegativeAttributes(this, reader);

                        _ReforgedPrefix = (ReforgedPrefix)reader.ReadInt();
                        _ReforgedSuffix = (ReforgedSuffix)reader.ReadInt();
                        _ItemPower = (ItemPower)reader.ReadInt();

                        _GorgonLenseCharges = reader.ReadInt();
                        _GorgonLenseType = (LenseType)reader.ReadInt();

                        _TimesImbued = reader.ReadEncodedInt();

                        _SAAbsorptionAttributes = new SAAbsorptionAttributes(this, reader);

                        _LastEquipped = reader.ReadBool();
                        _SetEquipped = reader.ReadBool();
                        _SetHue = reader.ReadEncodedInt();

                        _SetAttributes = new AosAttributes(this, reader);
                        _SetSkillBonuses = new AosSkillBonuses(this, reader);

                        _Crafter = reader.ReadMobile();
                        _Quality = (ItemQuality)reader.ReadInt();

                        _MaxHitPoints = reader.ReadEncodedInt();
                        _HitPoints = reader.ReadEncodedInt();

                        _Resource = (CraftResource)reader.ReadEncodedInt();
                        _GemType = (GemType)reader.ReadEncodedInt();

                        m_AosAttributes = new AosAttributes(this, reader);
                        m_AosResistances = new AosElementAttributes(this, reader);
                        _AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (Parent is Mobile)
                        {
                            _AosSkillBonuses.AddTo((Mobile)Parent);
                        }

                        int strBonus = m_AosAttributes.BonusStr;
                        int dexBonus = m_AosAttributes.BonusDex;
                        int intBonus = m_AosAttributes.BonusInt;

                        if (Parent is Mobile mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0))
                        {
                            Mobile m = mobile;

                            string modName = Serial.ToString();

                            if (strBonus != 0)
                            {
                                m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));
                            }

                            if (dexBonus != 0)
                            {
                                m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));
                            }

                            if (intBonus != 0)
                            {
                                m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
                            }
                        }

                        if (Parent is Mobile mob)
                        {
                            mob.CheckStatTimers();
                        }

                        break;
                    }
            }

            if (_NegativeAttributes == null)
            {
                _NegativeAttributes = new NegativeAttributes(this);
            }

            if (_TalismanProtection == null)
            {
                _TalismanProtection = new TalismanAttribute();
            }

            if (_SetAttributes == null)
            {
                _SetAttributes = new AosAttributes(this);
            }

            if (_SetSkillBonuses == null)
            {
                _SetSkillBonuses = new AosSkillBonuses(this);
            }
        }

        #region ICraftable Members

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            PlayerConstructed = true;

            Type resourceType = typeRes;

            if (resourceType == null)
            {
                resourceType = craftItem.Resources.GetAt(0).ItemType;
            }

            if (!craftItem.ForceNonExceptional)
            {
                Resource = CraftResources.GetFromType(resourceType);
            }

            if (1 < craftItem.Resources.Count)
            {
                resourceType = craftItem.Resources.GetAt(1).ItemType;

                if (resourceType == typeof(StarSapphire))
                {
                    GemType = GemType.StarSapphire;
                }
                else if (resourceType == typeof(Emerald))
                {
                    GemType = GemType.Emerald;
                }
                else if (resourceType == typeof(Sapphire))
                {
                    GemType = GemType.Sapphire;
                }
                else if (resourceType == typeof(Ruby))
                {
                    GemType = GemType.Ruby;
                }
                else if (resourceType == typeof(Citrine))
                {
                    GemType = GemType.Citrine;
                }
                else if (resourceType == typeof(Amethyst))
                {
                    GemType = GemType.Amethyst;
                }
                else if (resourceType == typeof(Tourmaline))
                {
                    GemType = GemType.Tourmaline;
                }
                else if (resourceType == typeof(Amber))
                {
                    GemType = GemType.Amber;
                }
                else if (resourceType == typeof(Diamond))
                {
                    GemType = GemType.Diamond;
                }
            }

            _Quality = (ItemQuality)quality;

            if (makersMark)
            {
                _Crafter = from;
            }

            return quality;
        }

        #endregion

        #region Mondain's Legacy Sets
        public override bool OnDragLift(Mobile from)
        {
            if (Parent is Mobile && from == Parent)
            {
                if (IsSetItem && _SetEquipped)
                {
                    SetHelper.RemoveSetBonus(from, SetID, this);
                }
            }

            return base.OnDragLift(from);
        }

        public virtual SetItem SetID => SetItem.None;
        public virtual int Pieces => 0;

        public virtual bool BardMasteryBonus => (SetID == SetItem.Virtuoso);

        public virtual bool MixedSet => false;

        public bool IsSetItem => SetID != SetItem.None;

        private int _SetHue;
        private bool _SetEquipped;
        private bool _LastEquipped;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetHue
        {
            get => _SetHue;
            set
            {
                _SetHue = value;
                InvalidateProperties();
            }
        }

        public bool SetEquipped
        {
            get => _SetEquipped;
            set => _SetEquipped = value;
        }

        public bool LastEquipped
        {
            get => _LastEquipped;
            set => _LastEquipped = value;
        }

        private int _SetPhysicalBonus, _SetFireBonus, _SetColdBonus, _SetPoisonBonus, _SetEnergyBonus;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPhysicalBonus
        {
            get => _SetPhysicalBonus;
            set
            {
                _SetPhysicalBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetFireBonus
        {
            get => _SetFireBonus;
            set
            {
                _SetFireBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetColdBonus
        {
            get => _SetColdBonus;
            set
            {
                _SetColdBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPoisonBonus
        {
            get => _SetPoisonBonus;
            set
            {
                _SetPoisonBonus = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetEnergyBonus
        {
            get => _SetEnergyBonus;
            set
            {
                _SetEnergyBonus = value;
                InvalidateProperties();
            }
        }

        private AosAttributes _SetAttributes;
        private AosSkillBonuses _SetSkillBonuses;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes SetAttributes
        {
            get => _SetAttributes;
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SetSkillBonuses
        {
            get => _SetSkillBonuses;
            set
            {
            }
        }

        public int SetResistBonus(ResistanceType resist)
        {
            switch (resist)
            {
                case ResistanceType.Physical: return PhysicalResistance;
                case ResistanceType.Fire: return FireResistance;
                case ResistanceType.Cold: return ColdResistance;
                case ResistanceType.Poison: return PoisonResistance;
                case ResistanceType.Energy: return EnergyResistance;
            }

            return 0;
        }
        #endregion
    }
}
