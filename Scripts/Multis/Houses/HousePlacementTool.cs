using Server.Gumps;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Targeting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    [Flipable(0x194B, 0x194C)]
    public class SurveyorsScope : HousePlacementTool
    {
        public override int LabelNumber => 1026475;  // surveyor's scope

        [Constructable]
        public SurveyorsScope()
            : base(0x194B)
        {
        }

        public SurveyorsScope(Serial serial)
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

    public class HousePlacementTool : Item
    {
        [Constructable]
        public HousePlacementTool()
            : this(0x14F6)
        {
        }

        [Constructable]
        public HousePlacementTool(int itemid)
            : base(itemid)
        {
            Weight = 3.0;
            LootType = LootType.Blessed;
        }

        public HousePlacementTool(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber => 1060651; // a house placement tool

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (from.Map == Map.TerMur && !Engines.Points.PointsSystem.QueensLoyalty.IsNoble(from))
                {
                    from.SendLocalizedMessage(1113713); // You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to build a house in her lands.
                    return;
                }

                from.SendGump(new HousePlacementListGump(this, from, HousePlacementEntry.PreBuiltHouses, true));
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public virtual void OnPlacement(BaseHouse house)
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

    public class HousePlacementListGump : Gump
    {
        private const int LabelColor = 0x7FFF;
        private const int LabelHue = 0x481;

        private readonly Mobile m_From;
        private readonly HousePlacementEntry[] m_Entries;
        private readonly HousePlacementTool m_Tool;

        private readonly bool m_Classic;

        public HousePlacementListGump(HousePlacementTool tool, Mobile from, HousePlacementEntry[] entries, bool classic = false)
            : base(50, 50)
        {
            m_From = from;
            m_Tool = tool;
            m_Entries = entries;
            m_Classic = classic;

            from.CloseGump(typeof(HousePlacementListGump));
            from.CloseGump(typeof(HouseSwapGump));

            AddPage(0);

            AddBackground(0, 0, 530, 430, 5054);

            AddImageTiled(10, 10, 500, 20, 2624);
            AddAlphaRegion(10, 10, 500, 20);

            AddHtmlLocalized(10, 10, 500, 20, 1060239, LabelColor, false, false); // <CENTER>HOUSE PLACEMENT TOOL</CENTER>

            AddImageTiled(10, 40, 500, 20, 2624);
            AddAlphaRegion(10, 40, 500, 20);

            AddHtmlLocalized(50, 40, 225, 20, 1060235, LabelColor, false, false); // House Description
            AddHtmlLocalized(275, 40, 75, 20, 1060236, LabelColor, false, false); // Storage
            AddHtmlLocalized(350, 40, 75, 20, 1060237, LabelColor, false, false); // Lockdowns
            AddHtmlLocalized(425, 40, 75, 20, 1060034, LabelColor, false, false); // Cost

            AddImageTiled(10, 70, 500, 280, 2624);
            AddAlphaRegion(10, 70, 500, 280);

            AddImageTiled(10, 370, 500, 20, 2624);
            AddAlphaRegion(10, 370, 500, 20);

            AddHtmlLocalized(10, 370, 250, 20, 1060645, LabelColor, false, false); // Bank Balance:
            AddLabel(250, 370, LabelHue, Banker.GetBalance(from).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

            AddImageTiled(10, 400, 500, 20, 2624);
            AddAlphaRegion(10, 400, 500, 20);

            AddButton(10, 400, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(50, 400, 100, 20, 3000363, LabelColor, false, false); // Close

            int page = 1;
            int index = -1;

            for (int i = 0; i < entries.Length; ++i)
            {
                CheckPage(i, ref page, ref index);

                if (index == 0)
                {
                    if (page > 1)
                    {
                        AddButton(450, 400, 4005, 4007, 0, GumpButtonType.Page, page);
                        AddHtmlLocalized(400, 400, 100, 20, 3000406, LabelColor, false, false); // Next
                    }

                    AddPage(page);

                    if (page > 1)
                    {
                        AddButton(200, 400, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                        AddHtmlLocalized(250, 400, 100, 20, 3000405, LabelColor, false, false); // Previous
                    }
                }

                HousePlacementEntry entry = entries[i];

                int y = 70 + index * 20;
                int storage = (int)(entry.Storage * BaseHouse.GlobalBonusStorageScalar);
                int lockdowns = (int)(entry.Lockdowns * BaseHouse.GlobalBonusStorageScalar);

                AddButton(10, y, 4005, 4007, 1 + i, GumpButtonType.Reply, 0);
                AddHtmlLocalized(50, y, 225, 20, entry.Description, LabelColor, false, false);
                AddLabel(275, y, LabelHue, storage.ToString());
                AddLabel(350, y, LabelHue, lockdowns.ToString());
                AddLabel(425, y, LabelHue, entry.Cost.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (!m_From.CheckAlive() || m_From.Backpack == null || !m_Tool.IsChildOf(m_From.Backpack))
            {
                return;
            }

            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_Entries.Length)
            {
                m_From.Target = new NewHousePlacementTarget(m_Tool, m_Entries, m_Entries[index], m_Classic);
            }
        }

        private void CheckPage(int i, ref int page, ref int index)
        {
            if (m_Classic)
            {
                if (i == 8)
                {
                    page = 2;
                    index = 0;
                }
                else if (i == 20)
                {
                    page = 3;
                    index = 0;
                }
                else if (i == 32)
                {
                    page = 4;
                    index = 0;
                }
                else if (i == 44)
                {
                    page = 5;
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
            else
            {
                page = 1 + i / 14;
                index = i % 14;
            }
        }
    }

    public class NewHousePlacementTarget : MultiTarget
    {
        private readonly HousePlacementEntry m_Entry;
        private readonly HousePlacementEntry[] m_Entries;

        private bool m_Placed;
        private readonly bool m_Classic;
        private readonly HousePlacementTool m_Tool;

        public NewHousePlacementTarget(HousePlacementTool tool, HousePlacementEntry[] entries, HousePlacementEntry entry, bool classic)
            : base(entry.MultiID, entry.Offset)
        {
            Range = 14;

            m_Tool = tool;
            m_Entries = entries;
            m_Entry = entry;
            m_Classic = classic;
        }

        protected override void OnTarget(Mobile from, object o)
        {
            if (!from.CheckAlive() || from.Backpack == null || !m_Tool.IsChildOf(from.Backpack))
            {
                return;
            }

            if (o is IPoint3D ip)
            {
                if (ip is Item item)
                {
                    ip = item.GetWorldTop();
                }

                Point3D p = new Point3D(ip);

                Region reg = Region.Find(new Point3D(p), from.Map);

                if (from.AccessLevel >= AccessLevel.GameMaster || reg.AllowHousing(from, p))
                {
                    m_Placed = m_Entry.OnPlacement(m_Tool, from, p);
                }
                else if (reg.IsPartOf<TempNoHousingRegion>())
                {
                    from.SendLocalizedMessage(501270); // Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
                }
                else if (reg.IsPartOf<HouseRegion>())
                {
                    from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
                }
                else
                {
                    from.SendLocalizedMessage(501265); // Housing can not be created in this area.
                }
            }
        }

        protected override void OnTargetFinish(Mobile from)
        {
            if (!from.CheckAlive() || from.Backpack == null || !m_Tool.IsChildOf(from.Backpack))
            {
                return;
            }

            if (!m_Placed)
            {
                from.SendGump(new HousePlacementListGump(m_Tool, from, m_Entries, m_Classic));
            }
        }
    }

    public class HousePlacementEntry
    {
        private static readonly HousePlacementEntry[] _PreBuiltHouses =
        {
            new(typeof(SmallOldHouse),      1011303,    425,    212,    489,    244,    10, 36750, 0,   4,  0,  0x0064),
            new(typeof(SmallOldHouse),      1011304,    425,    212,    489,    244,    10, 36750, 0,   4,  0,  0x0066),
            new(typeof(SmallOldHouse),      1011305,    425,    212,    489,    244,    10, 36500, 0,   4,  0,  0x0068),
            new(typeof(SmallOldHouse),      1011306,    425,    212,    489,    244,    10, 35000, 0,   4,  0,  0x006A),
            new(typeof(SmallOldHouse),      1011307,    425,    212,    489,    244,    10, 36500, 0,   4,  0,  0x006C),
            new(typeof(SmallOldHouse),      1011308,    425,    212,    489,    244,    10, 36500, 0,   4,  0,  0x006E),
            new(typeof(SmallShop),          1011321,    425,    212,    489,    244,    10, 50250, -1,  4,  0,  0x00A0),
            new(typeof(SmallShop),          1011322,    425,    212,    489,    244,    10, 52250, 0,   4,  0,  0x00A2),
            new(typeof(SmallTower),         1011317,    580,    290,    667,    333,    14, 73250, 3,   4,  0,  0x0098),
            new(typeof(TwoStoryVilla),      1011319,    1100,   550,    1265,   632,    24, 113500, 3,  6,  0,  0x009E),
            new(typeof(SandStonePatio),     1011320,    850,    425,    1265,   632,    24, 76250, -1,  4,  0,  0x009C),
            new(typeof(LogCabin),           1011318,    1100,   550,    1265,   632,    24, 81250, 1,   6,  0,  0x009A),
            new(typeof(GuildHouse),         1011309,    1370,   685,    1576,   788,    28, 131250, -1, 7,  0,  0x0074),
            new(typeof(TwoStoryHouse),      1011310,    1370,   685,    1576,   788,    28, 162500, -3, 7,  0,  0x0076),
            new(typeof(TwoStoryHouse),      1011311,    1370,   685,    1576,   788,    28, 162750, -3, 7,  0,  0x0078),
            new(typeof(LargePatioHouse),    1011315,    1370,   685,    1576,   788,    28, 129000, -4, 7,  0,  0x008C),
            new(typeof(LargeMarbleHouse),   1011316,    1370,   685,    1576,   788,    28, 160250, -4, 7,  0,  0x0096),
            new(typeof(Tower),              1011312,    2119,   1059,   2437,   1218,   42, 366250, 0,  7,  0,  0x007A),
            new(typeof(Keep),               1011313,    2625,   1312,   3019,   1509,   52, 562500, 0, 11,  0,  0x007C),
            new(typeof(Castle),             1011314,    4076,   2038,   4688,   2344,   78, 865000, 0, 16,  0,  0x007E),
        };

        private static readonly Hashtable m_Table;
        private readonly Type m_Type;
        private readonly int m_Description;
        private readonly int m_Storage;
        private readonly int m_Lockdowns;
        private readonly int m_NewStorage;
        private readonly int m_NewLockdowns;
        private readonly int m_Vendors;
        private readonly int m_Cost;
        private readonly int m_MultiID;
        private readonly Point3D m_Offset;

        public HousePlacementEntry(Type type, int description, int storage, int lockdowns, int newStorage, int newLockdowns, int vendors, int cost, int xOffset, int yOffset, int zOffset, int multiID)
        {
            m_Type = type;
            m_Description = description;
            m_Storage = storage;
            m_Lockdowns = lockdowns;
            m_NewStorage = newStorage;
            m_NewLockdowns = newLockdowns;
            m_Vendors = vendors;

            m_Cost = Siege.SiegeShard ? cost * 2 : cost;

            m_Offset = new Point3D(xOffset, yOffset, zOffset);

            m_MultiID = multiID;
        }

        static HousePlacementEntry()
        {
            m_Table = new Hashtable();

            FillTable(_PreBuiltHouses);
        }

        public static HousePlacementEntry[] PreBuiltHouses => _PreBuiltHouses;

        public Type Type => m_Type;
        public int Description => m_Description;
        public int Storage => m_NewStorage;
        public int Lockdowns => m_NewLockdowns;
        public int Vendors => m_Vendors;
        public int Cost => m_Cost;
        public int MultiID => m_MultiID;
        public Point3D Offset => m_Offset;
        public static HousePlacementEntry Find(BaseHouse house)
        {
            object obj = m_Table[house.GetType()];

            if (obj is HousePlacementEntry entry)
            {
                return entry;
            }

            if (obj is ArrayList list)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    HousePlacementEntry e = (HousePlacementEntry)list[i];

                    if (e.m_MultiID == house.ItemID)
                    {
                        return e;
                    }
                }
            }
            else if (obj is Hashtable table)
            {
                obj = table[house.ItemID];

                if (obj is HousePlacementEntry placementEntry)
                {
                    return placementEntry;
                }
            }

            return null;
        }

        public BaseHouse ConstructHouse(Mobile from)
        {
            try
            {
                object[] args;

                if (m_Type == typeof(HouseFoundation))
                {
                    args = [from, m_MultiID, m_Storage, m_Lockdowns];
                }
                else if (m_Type == typeof(SmallOldHouse) || m_Type == typeof(SmallShop) || m_Type == typeof(TwoStoryHouse))
                {
                    args = [from, m_MultiID];
                }
                else
                {
                    args = [from];
                }

                return Activator.CreateInstance(m_Type, args) as BaseHouse;
            }
            catch (Exception e)
            {
                Diagnostics.ExceptionLogging.LogException(e);
            }

            return null;
        }

        public void PlacementWarning_Callback(Mobile from, bool okay, object state)
        {
            object[] objs = (object[])state;

            PreviewHouse prevHouse = (PreviewHouse)objs[0];
            HousePlacementTool tool = objs[1] as HousePlacementTool;

            if (!from.CheckAlive() || from.Backpack == null || tool == null || !tool.IsChildOf(from.Backpack))
            {
                return;
            }

            if (!okay)
            {
                prevHouse.Delete();
                return;
            }

            if (prevHouse.Deleted)
            {
                /* Too much time has passed and the test house you created has been deleted.
                * Please try again!
                */
                from.SendGump(new NoticeGump(1060637, 30720, 1060647, 32512, 320, 180, null, null));

                return;
            }

            Point3D center = prevHouse.Location;
            Map map = prevHouse.Map;

            prevHouse.Delete();

            ArrayList toMove;
            //Point3D center = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
            HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

            switch (res)
            {
                case HousePlacementResult.Valid:
                    {
                        if (from.AccessLevel > AccessLevel.Player || BaseHouse.CheckAccountHouseLimit(from))
                        {
                            BaseHouse house = ConstructHouse(from);

                            if (house == null)
                            {
                                return;
                            }

                            house.Price = m_Cost;

                            if (from.AccessLevel >= AccessLevel.GameMaster)
                            {
                                from.SendMessage("{0} gold would have been withdrawn from your bank if you were not a GM.", m_Cost.ToString());
                            }
                            else
                            {
                                if (!Banker.Withdraw(from, m_Cost, true))
                                {
                                    house.Delete();
                                    from.SendLocalizedMessage(1060646); // You do not have the funds available in your bank box to purchase this house.  Try placing a smaller house, or adding gold or checks to your bank box.
                                    return;
                                }
                            }

                            house.MoveToWorld(center, from.Map);

                            if (house is HouseFoundation foundation)
                            {
                                foundation.OnPlacement();
                            }

                            for (int i = 0; i < toMove.Count; ++i)
                            {
                                object o = toMove[i];

                                if (o is Mobile mobile)
                                {
                                    mobile.Location = house.BanLocation;
                                }
                                else if (o is Item item)
                                {
                                    item.Location = house.BanLocation;
                                }
                            }

                            tool.OnPlacement(house);
                        }

                        break;
                    }
                case HousePlacementResult.BadItem:
                case HousePlacementResult.BadLand:
                case HousePlacementResult.BadStatic:
                case HousePlacementResult.BadRegionHidden:
                case HousePlacementResult.NoSurface:
                    {
                        from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
                        break;
                    }
                case HousePlacementResult.BadRegion:
                    {
                        from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
                        break;
                    }
                case HousePlacementResult.BadRegionTemp:
                    {
                        from.SendLocalizedMessage(501270); // Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
                        break;
                    }
                case HousePlacementResult.InvalidCastleKeep:
                    {
                        from.SendLocalizedMessage(1061122); // Castles and keeps cannot be created here.
                        break;
                    }
                case HousePlacementResult.NoQueenLoyalty:
                    {
                        from.SendLocalizedMessage(1113707, "10000"); // You must have at lease ~1_MIN~ loyalty to the Gargoyle Queen to place a house in Ter Mur.
                        break;
                    }
            }
        }

        public bool OnPlacement(HousePlacementTool tool, Mobile from, Point3D p)
        {
            if (!from.CheckAlive() || from.Backpack == null || !tool.IsChildOf(from.Backpack))
            {
                return false;
            }

            ArrayList toMove;
            Point3D center = new Point3D(p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z);
            HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

            switch (res)
            {
                case HousePlacementResult.Valid:
                    {
                        from.SendLocalizedMessage(1011576); // This is a valid location.

                        PreviewHouse prev = new PreviewHouse(m_MultiID);

                        MultiComponentList mcl = prev.Components;

                        Point3D banLoc = new Point3D(center.X + mcl.Min.X, center.Y + mcl.Max.Y + 1, center.Z);

                        for (int i = 0; i < mcl.List.Length; ++i)
                        {
                            MultiTileEntry entry = mcl.List[i];

                            int itemID = entry.m_ItemID;

                            if (itemID >= 0xBA3 && itemID <= 0xC0E)
                            {
                                banLoc = new Point3D(center.X + entry.m_OffsetX, center.Y + entry.m_OffsetY, center.Z);
                                break;
                            }
                        }

                        for (int i = 0; i < toMove.Count; ++i)
                        {
                            object o = toMove[i];

                            if (o is Mobile mobile)
                            {
                                mobile.Location = banLoc;
                            }
                            else if (o is Item item)
                            {
                                item.Location = banLoc;
                            }
                        }

                        prev.MoveToWorld(center, from.Map);

                        /* You are about to place a new house.
                        * Placing this house will condemn any and all of your other houses that you may have.
                        * All of your houses on all shards will be affected.
                        * 
                        * In addition, you will not be able to place another house or have one transferred to you for one (1) real-life week.
                        * 
                        * Once you accept these terms, these effects cannot be reversed.
                        * Re-deeding or transferring your new house will not uncondemn your other house(s) nor will the one week timer be removed.
                        * 
                        * If you are absolutely certain you wish to proceed, click the button next to OKAY below.
                        * If you do not wish to trade for this house, click CANCEL.
                        */
                        from.SendGump(new WarningGump(1060635, 30720, 1049583, 32512, 420, 280, PlacementWarning_Callback, new object[] { prev, tool }));

                        return true;
                    }
                case HousePlacementResult.BadItem:
                case HousePlacementResult.BadLand:
                case HousePlacementResult.BadStatic:
                case HousePlacementResult.BadRegionHidden:
                case HousePlacementResult.NoSurface:
                    {
                        from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
                        break;
                    }
                case HousePlacementResult.BadRegion:
                    {
                        from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
                        break;
                    }
                case HousePlacementResult.BadRegionTemp:
                    {
                        from.SendLocalizedMessage(501270); //Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
                        break;
                    }
                case HousePlacementResult.InvalidCastleKeep:
                    {
                        from.SendLocalizedMessage(1061122); // Castles and keeps cannot be created here.
                        break;
                    }
                case HousePlacementResult.NoQueenLoyalty:
                    {
                        from.SendLocalizedMessage(1113707, "10000"); // You must have at lease ~1_MIN~ loyalty to the Gargoyle Queen to place a house in Ter Mur.
                        break;
                    }
            }

            return false;
        }

        private static void FillTable(HousePlacementEntry[] entries)
        {
            for (int i = 0; i < entries.Length; ++i)
            {
                HousePlacementEntry e = entries[i];

                object obj = m_Table[e.m_Type];

                if (obj == null)
                {
                    m_Table[e.m_Type] = e;
                }
                else if (obj is HousePlacementEntry)
                {
                    ArrayList list = new ArrayList();

                    list.Add(obj);
                    list.Add(e);

                    m_Table[e.m_Type] = list;
                }
                else if (obj is ArrayList list)
                {
                    if (list.Count == 8)
                    {
                        Hashtable table = new Hashtable();

                        for (int j = 0; j < list.Count; ++j)
                        {
                            table[((HousePlacementEntry)list[j]).m_MultiID] = list[j];
                        }

                        table[e.m_MultiID] = e;

                        m_Table[e.m_Type] = table;
                    }
                    else
                    {
                        list.Add(e);
                    }
                }
                else if (obj is Hashtable hashtable)
                {
                    hashtable[e.m_MultiID] = e;
                }
            }
        }
    }

    public class HouseSwapGump : BaseGump
    {
        private const int LabelColor = 0x7FFF;
        private const int LabelHue = 0x481;

        private readonly Mobile m_From;
        private readonly HousePlacementEntry[] m_Entries;
        private readonly BaseHouse m_House;

        public HouseSwapGump(Mobile from, BaseHouse house, HousePlacementEntry[] entries)
            : base((PlayerMobile)from, 50, 50)
        {
            m_From = from;
            m_Entries = entries;
            m_House = house;

            from.CloseGump(typeof(HousePlacementListGump));
            from.CloseGump(typeof(HouseSwapGump));
        }

        public override void AddGumpLayout()
        {
            AddPage(0);

            AddBackground(0, 0, 530, 430, 5054);

            AddImageTiled(10, 10, 500, 20, 2624);
            AddAlphaRegion(10, 10, 500, 20);

            AddHtmlLocalized(10, 10, 500, 20, 1158759, LabelColor, false, false); // <CENTER>SECURE HOUSE REPLACEMENT</CENTER>

            AddImageTiled(10, 40, 500, 20, 2624);
            AddAlphaRegion(10, 40, 500, 20);

            AddHtmlLocalized(50, 40, 225, 20, 1060235, LabelColor, false, false); // House Description
            AddHtmlLocalized(275, 40, 75, 20, 1060236, LabelColor, false, false); // Storage
            AddHtmlLocalized(350, 40, 75, 20, 1060237, LabelColor, false, false); // Lockdowns
            AddHtmlLocalized(425, 40, 75, 20, 1060034, LabelColor, false, false); // Cost

            AddImageTiled(10, 70, 500, 280, 2624);
            AddAlphaRegion(10, 70, 500, 280);

            AddImageTiled(10, 370, 500, 20, 2624);
            AddAlphaRegion(10, 370, 500, 20);

            AddHtmlLocalized(10, 370, 250, 20, 1060645, LabelColor, false, false); // Bank Balance:
            AddLabel(250, 370, LabelHue, Banker.GetBalance(m_From).ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

            AddImageTiled(10, 400, 500, 20, 2624);
            AddAlphaRegion(10, 400, 500, 20);

            AddButton(10, 400, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(50, 400, 100, 20, 3000363, LabelColor, false, false); // Close

            for (int i = 0; i < m_Entries.Length; ++i)
            {
                int page = 1 + i / 14;
                int index = i % 14;

                if (index == 0)
                {
                    if (page > 1)
                    {
                        AddButton(450, 400, 4005, 4007, 0, GumpButtonType.Page, page);
                        AddHtmlLocalized(400, 400, 100, 20, 3000406, LabelColor, false, false); // Next
                    }

                    AddPage(page);

                    if (page > 1)
                    {
                        AddButton(200, 400, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                        AddHtmlLocalized(250, 400, 100, 20, 3000405, LabelColor, false, false); // Previous
                    }
                }

                HousePlacementEntry entry = m_Entries[i];

                int y = 70 + index * 20;
                int storage = (int)(entry.Storage * BaseHouse.GlobalBonusStorageScalar);
                int lockdowns = (int)(entry.Lockdowns * BaseHouse.GlobalBonusStorageScalar);

                AddButton(10, y, 4005, 4007, 1 + i, GumpButtonType.Reply, 0);
                AddHtmlLocalized(50, y, 225, 20, entry.Description, LabelColor, false, false);
                AddLabel(275, y, LabelHue, storage.ToString());
                AddLabel(350, y, LabelHue, lockdowns.ToString());
                AddLabel(425, y, LabelHue, entry.Cost.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (!m_From.CheckAlive() || m_From.Backpack == null || m_From.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
            {
                return;
            }

            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_Entries.Length)
            {
                HousePlacementEntry e = m_Entries[index];

                if (e != null)
                {
                    int cost = e.Cost - m_House.Price;

                    if (cost > 0)
                    {
                        if (!Banker.Withdraw(m_From, cost, true))
                        {
                            m_From.SendLocalizedMessage(
                                1061624); // You do not have enough funds in your bank to cover the difference between your old house and your new one.
                            return;
                        }
                    }
                    else if (cost < 0)
                    {
                        Banker.Deposit(m_From, -cost, true);
                    }

                    BaseHouse newHouse = e.ConstructHouse(m_From);

                    if (newHouse != null)
                    {
                        newHouse.Price = e.Cost;

                        m_House.MoveAllToCrate();

                        newHouse.Friends = new List<Mobile>(m_House.Friends);
                        newHouse.CoOwners = new List<Mobile>(m_House.CoOwners);
                        newHouse.Bans = new List<Mobile>(m_House.Bans);
                        newHouse.Access = new List<Mobile>(m_House.Access);
                        newHouse.BuiltOn = m_House.BuiltOn;
                        newHouse.LastTraded = m_House.LastTraded;
                        newHouse.Public = m_House.Public;

                        newHouse.VendorInventories.AddRange(m_House.VendorInventories);
                        m_House.VendorInventories.Clear();

                        for (var i = 0; i < newHouse.VendorInventories.Count; i++)
                        {
                            VendorInventory inventory = newHouse.VendorInventories[i];

                            inventory.House = newHouse;
                        }

                        newHouse.InternalizedVendors.AddRange(m_House.InternalizedVendors);
                        m_House.InternalizedVendors.Clear();

                        for (var i = 0; i < newHouse.InternalizedVendors.Count; i++)
                        {
                            Mobile mobile = newHouse.InternalizedVendors[i];

                            if (mobile is PlayerVendor playerVendor)
                            {
                                playerVendor.House = newHouse;
                            }
                            else if (mobile is PlayerBarkeeper playerBarkeeper)
                            {
                                playerBarkeeper.House = newHouse;
                            }
                        }

                        if (m_House.MovingCrate != null)
                        {
                            newHouse.MovingCrate = m_House.MovingCrate;
                            newHouse.MovingCrate.House = newHouse;
                            m_House.MovingCrate = null;
                        }

                        List<Item> items = m_House.GetItems();
                        List<Mobile> mobiles = m_House.GetMobiles();

                        newHouse.MoveToWorld(
                            new Point3D(m_House.X + m_House.ConvertOffsetX, m_House.Y + m_House.ConvertOffsetY,
                                m_House.Z + m_House.ConvertOffsetZ), m_House.Map);
                        m_House.Delete();

                        for (var i = 0; i < items.Count; i++)
                        {
                            Item item = items[i];

                            item.Location = newHouse.BanLocation;
                        }

                        for (var i = 0; i < mobiles.Count; i++)
                        {
                            Mobile mobile = mobiles[i];

                            mobile.Location = newHouse.BanLocation;
                        }

                        /* You have successfully replaced your original house with a new house.
                        * The value of the replaced house has been deposited into your bank box.
                        * All of the items in your original house have been relocated to a Moving Crate in the new house.
                        * Any deed-based house add-ons have been converted back into deeds.
                        * Vendors and barkeeps in the house, if any, have been stored in the Moving Crate as well.
                        * Use the <B>Get Vendor</B> context-sensitive menu option on your character to retrieve them.
                        * These containers can be used to re-create the vendor in a new location.
                        * Any barkeepers have been converted into deeds.
                        */
                        m_From.SendGump(new NoticeGump(1060637, 30720, 1060012, 32512, 420, 280, null, null));
                    }
                }
            }
            else
            {
                Refresh();
            }
        }
    }
}
