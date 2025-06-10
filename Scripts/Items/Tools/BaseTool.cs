using Server.Engines.Craft;
using System;

namespace Server.Items
{
    public interface ITool : IEntity, IUsesRemaining
    {
        CraftSystem CraftSystem { get; }

        bool BreakOnDepletion { get; }

        bool CheckAccessible(Mobile from, ref int num);
    }

    public abstract class BaseTool : Item, ITool, IResource, IQuality
    {
        private Mobile _Crafter;
        private ItemQuality _Quality;
        private int _UsesRemaining;
        private bool _RepairMode;
        private CraftResource _Resource;
        private bool _PlayerConstructed;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get => _Resource;
            set
            {
                _Resource = value;
                Hue = CraftResources.GetHue(_Resource);
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter { get => _Crafter; set { _Crafter = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemQuality Quality
        {
            get => _Quality;
            set
            {
                UnscaleUses();
                _Quality = value;
                InvalidateProperties();
                ScaleUses();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get => _PlayerConstructed;
            set
            {
                _PlayerConstructed = value; InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int UsesRemaining { get => _UsesRemaining; set { _UsesRemaining = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RepairMode { get => _RepairMode; set => _RepairMode = value; }

        public void ScaleUses()
        {
            _UsesRemaining = _UsesRemaining * GetUsesScalar() / 100;
            InvalidateProperties();
        }

        public void UnscaleUses()
        {
            _UsesRemaining = _UsesRemaining * 100 / GetUsesScalar();
        }

        public int GetUsesScalar()
        {
            if (_Quality == ItemQuality.Exceptional)
            {
                return 200;
            }

            return 100;
        }

        public bool ShowUsesRemaining { get => true; set { } }

        public virtual bool BreakOnDepletion => true;

        public abstract CraftSystem CraftSystem { get; }

        public BaseTool(int itemID)
            : this(Utility.RandomMinMax(25, 75), itemID)
        {
        }

        public BaseTool(int uses, int itemID)
            : base(itemID)
        {
            _UsesRemaining = uses;
            _Quality = ItemQuality.Normal;
        }

        public BaseTool(Serial serial)
            : base(serial)
        {
        }

        public override void AddCraftedProperties(ObjectPropertyList list)
        {
            if (_Crafter != null)
            {
                list.Add(1050043, _Crafter.TitleName); // crafted by ~1_NAME~
            }

            if (_Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // exceptional
            }
        }

        public override void AddUsesRemainingProperties(ObjectPropertyList list)
        {
            list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public virtual bool CheckAccessible(Mobile m, ref int num)
        {
            if (RootParent != m)
            {
                num = 1044263;
                return false;
            }

            return true;
        }

        public static bool CheckTool(Item tool, Mobile m)
        {
            if (tool == null || tool.Deleted)
            {
                return false;
            }

            Item check = m.FindItemOnLayer(Layer.OneHanded);

            if (check is ITool && check != tool && !(check is AncientSmithyHammer))
            {
                return false;
            }

            check = m.FindItemOnLayer(Layer.TwoHanded);

            if (check is ITool && check != tool && !(check is AncientSmithyHammer))
            {
                return false;
            }

            return true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                CraftSystem system = CraftSystem;

                if (_RepairMode)
                {
                    Repair.Do(from, system, this);
                }
                else
                {
                    int num = system.CanCraft(from, this, null);

                    if (num > 0 && num != 1044267) // Blacksmithing shows the gump regardless of proximity of an anvil and forge after SE
                    {
                        from.SendLocalizedMessage(num);
                    }
                    else
                    {
                        from.SendGump(new CraftGump(from, system, this, null));
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(_PlayerConstructed);
            writer.Write((int)_Resource);
            writer.Write(_RepairMode);
            writer.Write(_Crafter);
            writer.Write((int)_Quality);
            writer.Write(_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            _PlayerConstructed = reader.ReadBool();
            _Resource = (CraftResource)reader.ReadInt();
            _RepairMode = reader.ReadBool();
            _Crafter = reader.ReadMobile();
            _Quality = (ItemQuality)reader.ReadInt();
            _UsesRemaining = reader.ReadInt();
        }

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue)
        {
            PlayerConstructed = true;

            Quality = (ItemQuality)quality;

            if (makersMark)
            {
                Crafter = from;
            }

            return quality;
        }
    }
}
