namespace Server.Multis
{
    public class HouseFoundation : BaseHouse
    {
        public override Rectangle2D[] Area
        {
            get
            {
                MultiComponentList mcl = Components;

                return [new Rectangle2D(mcl.Min.X, mcl.Min.Y, mcl.Width, mcl.Height)];
            }
        }

        public override Point3D BaseBanLocation => new Point3D(Components.Min.X, Components.Height - 1 - Components.Center.Y, 0);

        public HouseFoundation(Serial serial)
            : base(serial)
        {
        }

        public override bool IsStairArea(IPoint3D p, out bool frontStairs)
        {
            if (p.Y >= Sign.Y)
            {
                frontStairs = true;
                return true;
            }

            frontStairs = false;
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }

        /* Stair block IDs
        * (sorted ascending)
        */
        private static readonly int[] m_BlockIDs =
        {
            0x3EE, 0x709, 0x71E, 0x721,
            0x738, 0x750, 0x76C, 0x788,
            0x7A3, 0x7BA, 0x35D2, 0x3609,
            0x4317, 0x4318, 0x4B07, 0x7807,
            0x9AEA, 0x9B4F
        };

        /* Stair sequence IDs
        * (sorted ascending)
        * Use this for stairs in the proper N,W,S,E sequence
        */
        private static readonly int[] m_StairSeqs =
        {
            0x3EF, 0x70A, 0x722, 0x739,
            0x751, 0x76D, 0x789, 0x7A4,
            0x9AEB, 0x9B50
        };

        /* Other stair IDs
        * Listed in order: north, west, south, east
        * Use this for stairs not in the proper sequence
        */
        private static readonly int[] m_StairIDs =
        {
            0x71F,  0x736,  0x737,  0x749,
            0x35D4, 0x35D3, 0x35D6, 0x35D5,
            0x360B, 0x360A, 0x360D, 0x360C,
            0x4360, 0x435E, 0x435F, 0x4361,
            0x435C, 0x435A, 0x435B, 0x435D,
            0x4364, 0x4362, 0x4363, 0x4365,
            0x4B05, 0x4B04, 0x4B34, 0x4B33,
            0x7809, 0x7808, 0x780A, 0x780B,
            0x7BB,  0x7BC, -1,      -1
        };

        private static readonly int[] m_CornerIDs =
        {
            0x749, 0x74A, 0x74B, 0x74C,
            0x4366, 0x4367, 0x4368, 0x4369,
            0x436A, 0x436B, 0x436C, 0x436D,
            0x4B01, 0x4B02, 0x4B03, 0x4B32,
            0x780C, 0x708D, 0x708E, 0x708F
        };

        public static bool IsStairBlock(int id)
        {
            int delta = -1;

            for (int i = 0; delta < 0 && i < m_BlockIDs.Length; ++i)
            {
                delta = m_BlockIDs[i] - id;
            }

            return delta == 0;
        }

        public static bool IsStair(int id)
        {
            bool any = false;

            for (var index = 0; index < m_StairSeqs.Length; index++)
            {
                var seq = m_StairSeqs[index];

                if (id >= seq && id <= seq + 8)
                {
                    any = true;
                    break;
                }
            }

            bool any1 = false;

            for (var index = 0; index < m_StairIDs.Length; index++)
            {
                var stairId = m_StairIDs[index];

                if (stairId == id)
                {
                    any1 = true;
                    break;
                }
            }

            bool any2 = false;

            for (var index = 0; index < m_CornerIDs.Length; index++)
            {
                var cornerId = m_CornerIDs[index];

                if (cornerId == id)
                {
                    any2 = true;
                    break;
                }
            }

            return any || any1 || any2;
        }
    }
}
