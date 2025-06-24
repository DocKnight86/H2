using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server
{
    public static class Siege
    {
        public static bool SiegeShard = Config.Get("Siege.IsSiege", false);

        public static int StatsPerDay = 15;

        public static Dictionary<PlayerMobile, int> StatsTable { get; }

        static Siege()
        {
            StatsTable = new Dictionary<PlayerMobile, int>();
        }

        public static bool CanGainStat(PlayerMobile m)
        {
            if (!StatsTable.ContainsKey(m))
            {
                return true;
            }

            return StatsTable[m] < StatsPerDay;
        }

        public static void CheckUsesRemaining(Mobile from, Item item)
        {
            if (item is IUsesRemaining uses)
            {
                uses.ShowUsesRemaining = true;
                uses.UsesRemaining--;

                if (uses.UsesRemaining <= 0)
                {
                    item.Delete();

                    from.SendLocalizedMessage(1044038); // You have worn out your tool!
                }
            }
        }
    }
}
