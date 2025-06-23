using System;
using Server.Gumps;
using Server.Menus;
using Server.Menus.Questions;
using Server.Multis;
using Server.Network;

namespace Server.Engines.Help
{
    public class ContainedMenu : QuestionMenu
    {
        private readonly Mobile m_From;
        public ContainedMenu(Mobile from)
            : base("You already have an open help request. We will have someone assist you as soon as possible.  What would you like to do?", new string[] { "Leave my old help request like it is.", "Remove my help request from the queue." })
        {
            m_From = from;
        }

        public override void OnCancel(NetState state)
        {
            m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
        }

        public override void OnResponse(NetState state, int index)
        {
            if (index == 0)
            {
                m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
            }
            else if (index == 1)
            {
                PageEntry entry = PageQueue.GetEntry(m_From);

                if (entry != null && entry.Handler == null)
                {
                    m_From.SendLocalizedMessage(1005307, "", 0x35); // Removed help request.
                    PageQueue.Remove(entry);
                }
                else
                {
                    m_From.SendLocalizedMessage(1005306, "", 0x35); // Help request unchanged.
                }
            }
        }
    }

    public class HelpGump : Gump
    {
        public HelpGump(Mobile from)
            : base(0, 0)
        {
            from.CloseGump(typeof(HelpGump));

            AddBackground(50, 25, 540, 430, 2600);

            AddPage(0);

            AddHtmlLocalized(150, 50, 360, 40, 1001002, false, false); // <CENTER><U>Ultima Online Help Menu</U></CENTER>
            AddButton(425, 415, 2073, 2072, 0, GumpButtonType.Reply, 0); // Close

            AddPage(1);

            AddButton(80, 170, 5540, 5541, 1, GumpButtonType.Reply, 0);
            AddHtml(110, 170, 450, 74, @"<u>My character is physically stuck in the game.</u> This choice only covers cases where your character is physically stuck in a location they cannot move out of. This option will only work two times in 24 hours.", true, true);
        }

        public static bool CheckCombat(Mobile m)
        {
            for (int i = 0; i < m.Aggressed.Count; ++i)
            {
                AggressorInfo info = m.Aggressed[i];

                if (DateTime.UtcNow - info.LastCombatTime < TimeSpan.FromSeconds(30.0))
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: // Close/Cancel
                    {
                        from.SendLocalizedMessage(501235, "", 0x35); // Help request aborted.

                        break;
                    }
                case 1: // Stuck
                    {
                        BaseHouse house = BaseHouse.FindHouseAt(from);

                        if (house != null)
                        {
                            from.Location = house.BanLocation;
                        }
                        else if (CityLoyalty.CityTradeSystem.HasTrade(from))
                        {
                            from.SendLocalizedMessage(1151733); // You cannot do that while carrying a Trade Order.
                        }
                        else if (from.Region.IsPartOf<Regions.Jail>())
                        {
                            from.SendLocalizedMessage(1114345, "", 0x35); // You'll need a better jailbreak plan than that!
                        }
                        else if (from.Region.CanUseStuckMenu(from) && !CheckCombat(from) && !from.Frozen && !from.Criminal)
                        {
                            StuckMenu menu = new StuckMenu(from, from, true);

                            menu.BeginClose();

                            from.SendGump(menu);
                        }
                        else
                        {
                            from.SendMessage("You cannot use this feature right now."); 
                        }

                        break;
                    }
            }
        }

        public static void HelpRequest(Mobile m)
        {
            foreach (Gump g in m.NetState.Gumps)
            {
                if (g is HelpGump)
                {
                    return;
                }
            }

            if (!PageQueue.CheckAllowedToPage(m))
            {
                return;
            }

            if (PageQueue.Contains(m))
            {
                m.SendMenu(new ContainedMenu(m));
            }
            else
            {
                m.SendGump(new HelpGump(m));
            }
        }
    }
}
