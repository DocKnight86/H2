using Server.Gumps;

namespace Server.Items
{
    public class HiddenTreasuresTreasureMap : TreasureMap
    {
        [Constructable]
        public HiddenTreasuresTreasureMap() : this(2, GetMap())
        {
        }

        [Constructable]
        public HiddenTreasuresTreasureMap(int level, Map map)
            : base(level, map)
        {
        }

        public HiddenTreasuresTreasureMap(int level, Map map, Point2D location)
        {
            Level = level;
            Facet = map;
            ChestLocation = location;
        }

        private static Map GetMap()
        {
            switch (Utility.Random(6))
            {
                default:
                case 0: return Map.Ilshenar;
                case 1: return Map.Malas;
                case 2: return Map.Tokuno;
                case 3: return Map.TerMur;
                case 4: return Map.Trammel;
                case 5: return Map.Felucca;
            }
        }

        public override void OnMapComplete(Mobile from, TreasureMapChest chest)
        {
            base.OnMapComplete(from, chest);

            if (chest != null)
                chest.DropItem(new StasisChamberActivator());
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1156811); // *A Special Map Thought to Lead to Lost Kotl Technology*
        }

        public HiddenTreasuresTreasureMap(Serial serial) : base(serial)
        {
        }

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

    public class SaltySeaMIB : MessageInABottle
    {
        [Constructable]
        public SaltySeaMIB() : this(Utility.RandomBool() ? Map.Trammel : Map.Felucca, 3)
        {
        }

        [Constructable]
        public SaltySeaMIB(Map map, int level)
            : base(map, level)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(GetWorldLocation(), 2) && from.InLOS(this))
            {
                Container cont = Parent as Container;
                SOS sos = new SaltySeaSOS(TargetMap, Level);
                Consume();

                if (cont != null)
                    cont.AddItem(sos);
                else
                    sos.MoveToWorld(Location, Map);

                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 501891); // You extract the message from the bottle.
            }
            else
            {
                from.SendLocalizedMessage(1019045); // I can't reach that.            
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1156806); // *Coordinates to a Shipwreck thought to have Lost Kotl Technology*
        }

        public SaltySeaMIB(Serial serial) : base(serial)
        {
        }

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

    public class SaltySeaSOS : SOS
    {
        [Constructable]
        public SaltySeaSOS(Map map, int level) : base(map, level)
        {
            //AssignLocation();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1156806); // *Coordinates to a Shipwreck thought to have Lost Kotl Technology*
        }

        public override void OnSOSComplete(Container chest)
        {
            base.OnSOSComplete(chest);

            if (chest != null)
                chest.DropItem(new StasisChamberRegulator());
        }

        public SaltySeaSOS(Serial serial) : base(serial)
        {
        }

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

    public class StasisChamberPowerCore : Item
    {
        public override int LabelNumber => 1156623;

        [Constructable]
        public StasisChamberPowerCore()
            : base(40155)
        {
        }

        public StasisChamberPowerCore(Serial serial) : base(serial)
        {
        }

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

    public class StasisChamberActivator : Item
    {
        public override int LabelNumber => 1156624;

        [Constructable]
        public StasisChamberActivator()
            : base(40158)
        {
        }

        public StasisChamberActivator(Serial serial)
            : base(serial)
        {
        }

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

    public class StasisChamberRegulator : Item
    {
        public override int LabelNumber => 1156626;

        [Constructable]
        public StasisChamberRegulator()
            : base(40157)
        {
        }

        public StasisChamberRegulator(Serial serial)
            : base(serial)
        {
        }

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

    public class UnabridgedAtlasOfEodon : Item
    {
        public override int LabelNumber => 1156721;  // Unabridged Atlas of Eodon

        [Constructable]
        public UnabridgedAtlasOfEodon()
            : base(7185)
        {
            Hue = 2007;
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (IsChildOf(m.Backpack))
            {
                Gump g = new Gump(100, 50);
                g.AddImage(0, 0, 30236);
                g.AddHtmlLocalized(115, 35, 350, 600, 1156723, 1, false, true);

                m.SendGump(g);
            }
        }

        public UnabridgedAtlasOfEodon(Serial serial)
            : base(serial)
        {
        }

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
