using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Engines.Craft;
using Server.Multis;
using Server.Network;
using Server.Network.Packets;
using Server.Spells;
using Server.Spells.Mysticism;
using Server.Targeting;

namespace Server.Items
{
    public enum SpellbookType
    {
        Invalid = -1,
        Regular,
        Necromancer,
        Paladin,
        Ninja,
        Samurai,
        Arcanist,
        Mystic,
        SkillMasteries
    }

    public enum BookQuality
    {
        Regular,
        Exceptional
    }

    public class Spellbook : Item, ICraftable, ISlayer, IEngravable, IVvVItem, IOwnerRestricted, IWearableDurability
    {
        private static readonly Dictionary<Mobile, List<Spellbook>> m_Table = new Dictionary<Mobile, List<Spellbook>>();

        private static readonly int[] m_LegendPropertyCounts =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 0 properties : 21/52 : 40%
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 1 property   : 15/52 : 29%
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2, // 2 properties : 10/52 : 19%
			3, 3, 3, 3, 3, 3 // 3 properties :  6/52 : 12%
		};

        private static readonly int[] m_ElderPropertyCounts =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 0 properties : 15/34 : 44%
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 1 property   : 10/34 : 29%
			2, 2, 2, 2, 2, 2, // 2 properties :  6/34 : 18%
			3, 3, 3 // 3 properties :  3/34 :  9%
		};

        private static readonly int[] m_GrandPropertyCounts =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 0 properties : 10/20 : 50%
			1, 1, 1, 1, 1, 1, // 1 property   :  6/20 : 30%
			2, 2, 2, // 2 properties :  3/20 : 15%
			3 // 3 properties :  1/20 :  5%
		};

        private static readonly int[] m_MasterPropertyCounts =
        {
            0, 0, 0, 0, 0, 0, // 0 properties : 6/10 : 60%
			1, 1, 1, // 1 property   : 3/10 : 30%
			2 // 2 properties : 1/10 : 10%
		};

        private static readonly int[] m_AdeptPropertyCounts =
        {
            0, 0, 0, // 0 properties : 3/4 : 75%
			1 // 1 property   : 1/4 : 25%
		};

        private string _EngravedText;
        private BookQuality _Quality;
        private AosAttributes _AosAttributes;
        private AosSkillBonuses _AosSkillBonuses;
        private NegativeAttributes _NegativeAttributes;
        private ulong _Content;
        private int _Count;
        private Mobile _Crafter;
        private SlayerName _Slayer;
        private SlayerName _Slayer2;

        [Constructable]
        public Spellbook()
            : this((ulong)0)
        { }

        [Constructable]
        public Spellbook(ulong content)
            : this(content, 0xEFA)
        { }

        public Spellbook(ulong content, int itemID)
            : base(itemID)
        {
            _AosAttributes = new AosAttributes(this);
            _AosSkillBonuses = new AosSkillBonuses(this);
            _NegativeAttributes = new NegativeAttributes(this);

            Weight = 3.0;
            Layer = Layer.OneHanded;
            LootType = LootType.Blessed;

            Content = content;
        }

        public Spellbook(Serial serial)
            : base(serial)
        { }

        [CommandProperty(AccessLevel.GameMaster)]
        public string EngravedText
        {
            get => _EngravedText;
            set
            {
                _EngravedText = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BookQuality Quality
        {
            get => _Quality;
            set
            {
                _Quality = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes { get => _AosAttributes; set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses { get => _AosSkillBonuses; set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public NegativeAttributes NegativeAttributes { get => _NegativeAttributes; set { } }

        public virtual SpellbookType SpellbookType => SpellbookType.Regular;
        public virtual int BookOffset => 0;
        public virtual int BookCount => 64;

        [CommandProperty(AccessLevel.GameMaster)]
        public ulong Content
        {
            get => _Content;
            set
            {
                if (_Content != value)
                {
                    _Content = value;

                    _Count = 0;

                    while (value > 0)
                    {
                        _Count += (int)(value & 0x1);
                        value >>= 1;
                    }

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SpellCount => _Count;

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
        public SlayerName Slayer
        {
            get => _Slayer;
            set
            {
                _Slayer = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SlayerName Slayer2
        {
            get => _Slayer2;
            set
            {
                _Slayer2 = value;
                InvalidateProperties();
            }
        }

        private bool _VvVItem;
        private Mobile _Owner;
        private string _OwnerName;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsVvVItem { get => _VvVItem; set { _VvVItem = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get => _Owner; set { _Owner = value; if (_Owner != null)
            {
                _OwnerName = _Owner.Name;
            }

            InvalidateProperties(); } }

        public virtual string OwnerName { get => _OwnerName; set { _OwnerName = value; InvalidateProperties(); } }

        private int _MaxHitPoints;
        private int _HitPoints;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get => _HitPoints;
            set
            {
                if (_HitPoints == value)
                {
                    return;
                }

                if (value > _MaxHitPoints)
                {
                    value = _MaxHitPoints;
                }

                _HitPoints = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get => _MaxHitPoints;
            set
            {
                _MaxHitPoints = value;
                InvalidateProperties();
            }
        }

        public virtual bool CanFortify => false;

        public virtual int InitMinHits => 0;
        public virtual int InitMaxHits => 0;

        public virtual void ScaleDurability()
        {
        }

        public virtual void UnscaleDurability()
        {
        }

        public virtual int OnHit(BaseWeapon weap, int damage)
        {
            if (_MaxHitPoints == 0)
            {
                return damage;
            }

            int chance = _NegativeAttributes.Antique > 0 ? 50 : 25;

            if (chance > Utility.Random(100)) // 25% chance to lower durability
            {
                if (_HitPoints >= 1)
                {
                    HitPoints--;
                }
                else if (_MaxHitPoints > 0)
                {
                    MaxHitPoints--;

                    if (Parent is Mobile mobile)
                    {
                        mobile.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                    }

                    if (_MaxHitPoints == 0)
                    {
                        Delete();
                    }
                }
            }

            return damage;
        }

        public static void Initialize()
        {
            CommandSystem.Register("AllSpells", AccessLevel.GameMaster, AllSpells_OnCommand);
        }

        public static void TargetedSpell(Mobile from, IEntity target, int spellId)
        {
            try
            {
                if (!DesignContext.Check(from))
                {
                    return; // They are customizing
                }

                Spellbook book = Find(from, spellId);

                if (book != null && book.HasSpell(spellId))
                {
                    SpecialMove move = SpellRegistry.GetSpecialMove(spellId);

                    if (move != null)
                    {
                        SpecialMove.SetCurrentMove(from, move);
                    }
                    else if (target != null)
                    {
                        Mobile to = World.FindMobile(target.Serial);
                        Item toI = World.FindItem(target.Serial);
                        Spell spell = SpellRegistry.NewSpell(spellId, from, null);

                        if (spell != null && !Spells.SkillMasteries.MasteryInfo.IsPassiveMastery(spellId))
                        {
                            if (to != null)
                            {
                                spell.InstantTarget = to;
                            }
                            else if (toI != null)
                            {
                                spell.InstantTarget = toI as IEntity;
                            }

                            spell.Cast();
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500015); // You do not have that spell!
                }
            }
            catch (Exception ex)
            {
                Diagnostics.ExceptionLogging.LogException(ex);
            }
        }

        public static SpellbookType GetTypeForSpell(int spellID)
        {
            if (spellID >= 0 && spellID < 64)
            {
                return SpellbookType.Regular;
            }

            if (spellID >= 100 && spellID < 117)
            {
                return SpellbookType.Necromancer;
            }

            if (spellID >= 200 && spellID < 210)
            {
                return SpellbookType.Paladin;
            }

            if (spellID >= 400 && spellID < 406)
            {
                return SpellbookType.Samurai;
            }

            if (spellID >= 500 && spellID < 508)
            {
                return SpellbookType.Ninja;
            }

            if (spellID >= 600 && spellID < 617)
            {
                return SpellbookType.Arcanist;
            }

            if (spellID >= 677 && spellID < 693)
            {
                return SpellbookType.Mystic;
            }

            if (spellID >= 700 && spellID < 746)
            {
                return SpellbookType.SkillMasteries;
            }

            return SpellbookType.Invalid;
        }

        public static Spellbook FindRegular(Mobile from)
        {
            return Find(from, -1, SpellbookType.Regular);
        }

        public static Spellbook FindNecromancer(Mobile from)
        {
            return Find(from, -1, SpellbookType.Necromancer);
        }

        public static Spellbook FindPaladin(Mobile from)
        {
            return Find(from, -1, SpellbookType.Paladin);
        }

        public static Spellbook FindSamurai(Mobile from)
        {
            return Find(from, -1, SpellbookType.Samurai);
        }

        public static Spellbook FindNinja(Mobile from)
        {
            return Find(from, -1, SpellbookType.Ninja);
        }

        public static Spellbook FindArcanist(Mobile from)
        {
            return Find(from, -1, SpellbookType.Arcanist);
        }

        public static Spellbook FindMystic(Mobile from)
        {
            return Find(from, -1, SpellbookType.Mystic);
        }

        public static Spellbook Find(Mobile from, int spellID)
        {
            return Find(from, spellID, GetTypeForSpell(spellID));
        }

        public static Spellbook Find(Mobile from, int spellID, SpellbookType type)
        {
            if (from == null)
            {
                return null;
            }

            if (from.Deleted)
            {
                m_Table.Remove(from);
                return null;
            }

            List<Spellbook> list = null;

            m_Table.TryGetValue(from, out list);

            bool searchAgain = false;

            if (list == null)
            {
                m_Table[from] = list = FindAllSpellbooks(from);
            }
            else
            {
                searchAgain = true;
            }

            Spellbook book = FindSpellbookInList(list, from, spellID, type);

            if (book == null && searchAgain)
            {
                m_Table[from] = list = FindAllSpellbooks(from);

                book = FindSpellbookInList(list, from, spellID, type);
            }

            return book;
        }

        public static Spellbook FindSpellbookInList(List<Spellbook> list, Mobile from, int spellID, SpellbookType type)
        {
            Container pack = from.Backpack;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (i >= list.Count)
                {
                    continue;
                }

                Spellbook book = list[i];

                if (!book.Deleted && (book.Parent == from || pack != null && book.Parent == pack || book.Parent is SpellbookStrap strap && strap.Parent == pack) && ValidateSpellbook(book, spellID, type))
                {
                    return book;
                }

                list.RemoveAt(i);
            }

            return null;
        }

        public static List<Spellbook> FindAllSpellbooks(Mobile from)
        {
            List<Spellbook> list = new List<Spellbook>();

            Item item = from.FindItemOnLayer(Layer.OneHanded);

            if (item is Spellbook book)
            {
                list.Add(book);
            }

            Container pack = from.Backpack;

            if (pack == null)
            {
                return list;
            }

            for (int i = 0; i < pack.Items.Count; ++i)
            {
                item = pack.Items[i];

                if (item is Spellbook spellbook)
                {
                    list.Add(spellbook);
                }

                if (item is SpellbookStrap strap)
                {
                    for (int s = 0; s < strap.Items.Count; ++s)
                    {
                        item = strap.Items[s];

                        if (item is Spellbook strapbook)
                        {
                            list.Add(strapbook);
                        }
                    }
                }
            }

            return list;
        }

        public static Spellbook FindEquippedSpellbook(Mobile from)
        {
            return from.FindItemOnLayer(Layer.OneHanded) as Spellbook;
        }

        public static bool ValidateSpellbook(Spellbook book, int spellID, SpellbookType type)
        {
            return book.SpellbookType == type && (spellID == -1 || book.HasSpell(spellID));
        }

        public override bool CanEquip(Mobile from)
        {
            if (!from.CanBeginAction(typeof(BaseWeapon)))
            {
                return false;
            }

            if (_Owner != null && _Owner != from)
            {
                from.SendLocalizedMessage(501023); // You must be the owner to use this item.
                return false;
            }

            if (IsVvVItem && !Engines.VvV.ViceVsVirtueSystem.IsVvV(from))
            {
                from.SendLocalizedMessage(1155496); // This item can only be used by VvV participants!
                return false;
            }

            return base.CanEquip(from);
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            return true;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is SpellScroll scroll && !(scroll is SpellStone))
            {
                SpellbookType type = GetTypeForSpell(scroll.SpellID);

                if (type != SpellbookType)
                {
                    return false;
                }

                if (HasSpell(scroll.SpellID))
                {
                    from.SendLocalizedMessage(500179); // That spell is already present in that spellbook.
                    return false;
                }

                int val = scroll.SpellID - BookOffset;

                if (val >= 0 && val < BookCount)
                {
                    from.Send(new PlaySound(0x249, GetWorldLocation()));

                    _Content |= (ulong)1 << val;
                    ++_Count;

                    if (scroll.Amount > 1)
                    {
                        scroll.Amount--;
                        return base.OnDragDrop(from, scroll);
                    }

                    InvalidateProperties();
                    scroll.Delete();
                    return true;
                }
                return false;
            }
            return false;
        }

        public override void OnAfterDuped(Item newItem)
        {
            Spellbook book = newItem as Spellbook;

            if (book == null)
            {
                return;
            }

            book._AosAttributes = new AosAttributes(newItem, _AosAttributes);
            book._AosSkillBonuses = new AosSkillBonuses(newItem, _AosSkillBonuses);
            book._NegativeAttributes = new NegativeAttributes(newItem, _NegativeAttributes);

            base.OnAfterDuped(newItem);
        }

        public override void OnAdded(object parent)
        {
            if (parent is Mobile from)
            {
                _AosSkillBonuses.AddTo(from);

                int strBonus = _AosAttributes.BonusStr;
                int dexBonus = _AosAttributes.BonusDex;
                int intBonus = _AosAttributes.BonusInt;

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
            }
        }

        public bool HasSpell(int spellID)
        {
            spellID -= BookOffset;

            return spellID >= 0 && spellID < BookCount && (_Content & ((ulong)1 << spellID)) != 0;
        }

        public void DisplayTo(Mobile to)
        {
            // The client must know about the spellbook or it will crash!
            NetState ns = to.NetState;

            if (ns == null)
            {
                return;
            }

            if (Parent == null)
            {
                to.Send(WorldPacket);
            }
            else if (Parent is Item)
            {
                to.Send(new ContainerContentUpdate(this));
            }
            else if (Parent is Mobile)
            {
                // What will happen if the client doesn't know about our parent?
                to.Send(new EquipUpdate(this));
            }

            to.Send(new DisplaySpellbookPacket(this));

            to.Send(new SpellbookContentPacket(this, ItemID, BookOffset + 1, _Content));
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (_Quality == BookQuality.Exceptional)
            {
                list.Add(1063341); // exceptional
            }

            if (_EngravedText != null)
            {
                list.Add(1072305, Utility.FixHtml(_EngravedText)); // Engraved: ~1_INSCRIPTION~
            }

            if (_Crafter != null)
            {
                list.Add(1050043, _Crafter.TitleName); // crafted by ~1_NAME~
            }

            if (IsVvVItem)
            {
                list.Add(1154937); // VvV Item
            }

            if (OwnerName != null)
            {
                list.Add(1153213, OwnerName);
            }

            if (_NegativeAttributes != null)
            {
                _NegativeAttributes.GetProperties(list, this);
            }

            _AosSkillBonuses.GetProperties(list);

            if (_Slayer != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(_Slayer);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            if (_Slayer2 != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(_Slayer2);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            int prop;

            if ((prop = _AosAttributes.SpellChanneling) != 0)
            {
                list.Add(1060482); // spell channeling
            }

            if ((prop = _AosAttributes.NightSight) != 0)
            {
                list.Add(1060441); // night sight
            }

            if ((prop = _AosAttributes.BonusStr) != 0)
            {
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~
            }

            if ((prop = _AosAttributes.BonusDex) != 0)
            {
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~
            }

            if ((prop = _AosAttributes.BonusInt) != 0)
            {
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~
            }

            if ((prop = _AosAttributes.BonusHits) != 0)
            {
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~
            }

            if ((prop = _AosAttributes.BonusStam) != 0)
            {
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~
            }

            if ((prop = _AosAttributes.BonusMana) != 0)
            {
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~
            }

            if ((prop = _AosAttributes.RegenHits) != 0)
            {
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~
            }

            if ((prop = _AosAttributes.RegenStam) != 0)
            {
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~
            }

            if ((prop = _AosAttributes.RegenMana) != 0)
            {
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~
            }

            if ((prop = _AosAttributes.EnhancePotions) != 0)
            {
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%
            }

            if ((prop = _AosAttributes.ReflectPhysical) != 0)
            {
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%
            }

            if ((prop = _AosAttributes.AttackChance) != 0)
            {
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%
            }

            if ((prop = _AosAttributes.WeaponSpeed) != 0)
            {
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%
            }

            if ((prop = _AosAttributes.WeaponDamage) != 0)
            {
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%
            }

            if ((prop = _AosAttributes.DefendChance) != 0)
            {
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%
            }

            if ((prop = _AosAttributes.CastRecovery) != 0)
            {
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~
            }

            if ((prop = _AosAttributes.CastSpeed) != 0)
            {
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~
            }

            if ((prop = _AosAttributes.SpellDamage) != 0)
            {
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%
            }

            if ((prop = _AosAttributes.LowerManaCost) != 0)
            {
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%
            }

            if ((prop = _AosAttributes.LowerRegCost) != 0)
            {
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%
            }

            if ((prop = _AosAttributes.IncreasedKarmaLoss) != 0)
            {
                list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%
            }

            AddProperty(list);

            list.Add(1042886, _Count.ToString()); // ~1_NUMBERS_OF_SPELLS~ Spells

            if (_MaxHitPoints > 0)
            {
                list.Add(1060639, "{0}\t{1}", _HitPoints, _MaxHitPoints); // durability ~1_val~ / ~2_val~
            }
        }

        public virtual void AddProperty(ObjectPropertyList list)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            Container pack = from.Backpack;

            if (Parent == from || pack != null && Parent == pack || Parent is SpellbookStrap strap && strap.Parent == pack)
            {
                DisplayTo(from);
            }
            else
            {
                from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            _NegativeAttributes.Serialize(writer);
            writer.Write(_HitPoints);
            writer.Write(_MaxHitPoints);
            writer.Write(_VvVItem);
            writer.Write(_Owner);
            writer.Write(_OwnerName);
            writer.Write((byte)_Quality);
            writer.Write(_EngravedText);
            writer.Write(_Crafter);
            writer.Write((int)_Slayer);
            writer.Write((int)_Slayer2);
            _AosAttributes.Serialize(writer);
            _AosSkillBonuses.Serialize(writer);
            writer.Write(_Content);
            writer.Write(_Count);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        _NegativeAttributes = new NegativeAttributes(this, reader);
                        _MaxHitPoints = reader.ReadInt();
                        _HitPoints = reader.ReadInt();
                        _VvVItem = reader.ReadBool();
                        _Owner = reader.ReadMobile();
                        _OwnerName = reader.ReadString();
                        _Quality = (BookQuality)reader.ReadByte();
                        _EngravedText = reader.ReadString();
                        _Crafter = reader.ReadMobile();
                        _Slayer = (SlayerName)reader.ReadInt();
                        _Slayer2 = (SlayerName)reader.ReadInt();
                        _AosAttributes = new AosAttributes(this, reader);
                        _AosSkillBonuses = new AosSkillBonuses(this, reader);
                        _Content = reader.ReadULong();
                        _Count = reader.ReadInt();

                        break;
                    }
            }

            if (_AosAttributes == null)
            {
                _AosAttributes = new AosAttributes(this);
            }

            if (_AosSkillBonuses == null)
            {
                _AosSkillBonuses = new AosSkillBonuses(this);
            }

            if (_NegativeAttributes == null)
            {
                _NegativeAttributes = new NegativeAttributes(this);
            }

            if (Parent is Mobile)
            {
                _AosSkillBonuses.AddTo((Mobile)Parent);
            }

            int strBonus = _AosAttributes.BonusStr;
            int dexBonus = _AosAttributes.BonusDex;
            int intBonus = _AosAttributes.BonusInt;

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
        }

        public double GetBookBaseSkill(Spellbook book, Mobile from)
        {
            double skill;

            switch(book.SpellbookType)
            {
                default:
                case SpellbookType.Regular: skill = from.Skills.Magery.BaseFixedPoint; break;
                case SpellbookType.Necromancer: skill = from.Skills.Necromancy.BaseFixedPoint; break;
                case SpellbookType.Mystic: skill = from.Skills.Mysticism.BaseFixedPoint; break;
            }

            return skill;
        }

        public virtual int OnCraft(
            int quality,
            bool makersMark,
            Mobile from,
            CraftSystem craftSystem,
            Type typeRes,
            ITool tool,
            CraftItem craftItem,
            int resHue)
        {
            double skill = GetBookBaseSkill(this, from);

            if (skill >= 800)
            {
                int[] propertyCounts;
                int minIntensity;
                int maxIntensity;

                if (skill >= 1000)
                {
                    if (skill >= 1200)
                    {
                        propertyCounts = m_LegendPropertyCounts;
                    }
                    else if (skill >= 1100)
                    {
                        propertyCounts = m_ElderPropertyCounts;
                    }
                    else
                    {
                        propertyCounts = m_GrandPropertyCounts;
                    }

                    minIntensity = 55;
                    maxIntensity = 75;
                }
                else if (skill >= 900)
                {
                    propertyCounts = m_MasterPropertyCounts;
                    minIntensity = 25;
                    maxIntensity = 45;
                }
                else
                {
                    propertyCounts = m_AdeptPropertyCounts;
                    minIntensity = 0;
                    maxIntensity = 15;
                }

                int propertyCount = propertyCounts[Utility.Random(propertyCounts.Length)];

                if (from.FindItemOnLayer(Layer.Talisman) is GuaranteedSpellbookImprovementTalisman talisman && talisman.Charges > 0)
                {
                    propertyCount++;
                    talisman.Charges--;

                    from.SendLocalizedMessage(1157210); // Your talisman magically improves your spellbook.

                    if (talisman.Charges <= 0)
                    {
                        from.SendLocalizedMessage(1157211); // Your talisman has been destroyed.
                        talisman.Delete();
                    }
                }

                BaseRunicTool.ApplyAttributesTo(this, true, propertyCount, minIntensity, maxIntensity);
            }

            if (makersMark)
            {
                Crafter = from;
            }

            _Quality = (BookQuality)(quality - 1);

            return quality;
        }

        [Usage("AllSpells")]
        [Description("Completely fills a targeted spellbook with scrolls.")]
        private static void AllSpells_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, AllSpells_OnTarget);
            e.Mobile.SendMessage("Target the spellbook to fill.");
        }

        private static void AllSpells_OnTarget(Mobile from, object obj)
        {
            if (obj is Spellbook book)
            {
                if (book.BookCount == 64)
                {
                    book.Content = ulong.MaxValue;
                }
                else
                {
                    book.Content = (1ul << book.BookCount) - 1;
                }

                from.SendMessage("The spellbook has been filled.");

                CommandLogging.WriteLine(
                    from, "{0} {1} filling spellbook {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(book));
            }
            else
            {
                from.BeginTarget(-1, false, TargetFlags.None, AllSpells_OnTarget);
                from.SendMessage("That is not a spellbook. Try again.");
            }
        }

        public static void OpenSpellbookRequest(Mobile from, int bookType)
        {
            if (!DesignContext.Check(from))
            {
                return; // They are customizing
            }

            SpellbookType type;

            switch (bookType)
            {
                default:
                case 1:
                    type = SpellbookType.Regular;
                    break;
                case 2:
                    type = SpellbookType.Necromancer;
                    break;
                case 3:
                    type = SpellbookType.Paladin;
                    break;
                case 4:
                    type = SpellbookType.Ninja;
                    break;
                case 5:
                    type = SpellbookType.Samurai;
                    break;
                case 6:
                    type = SpellbookType.Arcanist;
                    break;
                case 7:
                    type = SpellbookType.Mystic;
                    break;
            }

            Spellbook book = Find(from, -1, type);

            if (book != null)
            {
                book.DisplayTo(from);
            }
        }

        public static void CastSpellRequest(Mobile from, int spellID, Item item)
        {
            if (!DesignContext.Check(from))
            {
                return; // They are customizing
            }

            Spellbook book = item as Spellbook;

            if (book == null || !book.HasSpell(spellID))
            {
                book = Find(from, spellID);
            }

            if (book != null && book.HasSpell(spellID))
            {
                SpecialMove move = SpellRegistry.GetSpecialMove(spellID);

                if (move != null)
                {
                    SpecialMove.SetCurrentMove(from, move);
                }
                else
                {
                    Spell spell = SpellRegistry.NewSpell(spellID, from, null);

                    if (spell != null)
                    {
                        spell.Cast();
                    }
                    else if (!Spells.SkillMasteries.MasteryInfo.IsPassiveMastery(spellID))
                    {
                        from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }
    }
}
