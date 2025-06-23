using Server.ContextMenus;
using Server.Gumps;
using System;
using System.Collections.Generic;

namespace Server.Multis
{
    public class HouseSign : Item
    {
        private BaseHouse _Owner;

        public HouseSign(BaseHouse owner)
            : base(0xBD2)
        {
            _Owner = owner;
            Movable = false;
        }

        public HouseSign(Serial serial)
            : base(serial)
        {
        }

        public string GetName()
        {
            if (Name == null)
            {
                return "An Unnamed House";
            }

            return Name;
        }

        public BaseHouse Owner => _Owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RestrictDecay
        {
            get => _Owner != null && _Owner.RestrictDecay;
            set
            {
                if (_Owner != null)
                {
                    _Owner.RestrictDecay = value;
                }
            }
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (_Owner != null && !_Owner.Deleted)
            {
                _Owner.Delete();
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add(1061638); // A House Sign
        }

        public override bool ForceShowProperties => true;

        private bool m_GettingProperties;

        public bool GettingProperties => m_GettingProperties;

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1061639, Utility.FixHtml(GetName())); // Name: ~1_NAME~
            list.Add(1061640, _Owner == null || _Owner.Owner == null ? "nobody" : _Owner.Owner.Name); // Owner: ~1_OWNER~

            if (_Owner != null)
            {
                list.Add(_Owner.Public ? 1061641 : 1061642); // This House is Open to the Public : This is a Private Home

                m_GettingProperties = true;
                DecayLevel level = _Owner.DecayLevel;
                m_GettingProperties = false;

                if (level == DecayLevel.DemolitionPending)
                {
                    list.Add(1062497); // Demolition Pending
                }
                else if (level != DecayLevel.Ageless)
                {
                    if (level == DecayLevel.Collapsed)
                    {
                        level = DecayLevel.IDOC;
                    }

                    list.Add(1062028, $"#{1043009 + (int)level}"); // Condition: This structure is ...
                }
            }
        }

        public void ShowSign(Mobile m)
        {
            if (_Owner != null && m.AccessLevel == AccessLevel.Player && _Owner.IsFriend(m))
            {
                _Owner.RefreshDecay();
            }

            m.SendGump(new HouseGump(HouseGumpPage.Information, m, _Owner));
        }

        public void ClaimGump_Callback(Mobile from, bool okay, object state)
        {
            if (okay && _Owner != null && _Owner.Owner == null && _Owner.DecayLevel != DecayLevel.DemolitionPending)
            {
                bool canClaim = false;

                if (_Owner.CoOwners == null || _Owner.CoOwners.Count == 0)
                {
                    canClaim = _Owner.IsFriend(from);
                }
                else
                {
                    canClaim = _Owner.IsCoOwner(from);
                }

                if (canClaim && !BaseHouse.AtAccountHouseLimit(from))
                {
                    _Owner.Owner = from;
                    _Owner.LastTraded = DateTime.UtcNow;
                }
            }

            InvalidateProperties();
            ShowSign(from);
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (_Owner == null)
            {
                return;
            }

            if (m.AccessLevel < AccessLevel.GameMaster && _Owner.Owner == null && _Owner.DecayLevel != DecayLevel.DemolitionPending)
            {
                bool canClaim = false;

                if (_Owner.CoOwners == null || _Owner.CoOwners.Count == 0)
                {
                    canClaim = _Owner.IsFriend(m);
                }
                else
                {
                    canClaim = _Owner.IsCoOwner(m);
                }

                if (canClaim && !BaseHouse.AtAccountHouseLimit(m))
                {
                    /* You do not currently own any house on any shard with this account,
                    * and this house currently does not have an owner.  If you wish, you
                    * may choose to claim this house and become its rightful owner.  If
                    * you do this, it will become your Primary house and automatically
                    * refresh.  If you claim this house, you will be unable to place
                    * another house or have another house transferred to you for the
                    * next 7 days.  Do you wish to claim this house?
                    */
                    m.SendGump(new WarningGump(501036, 32512, 1049719, 32512, 420, 280, ClaimGump_Callback, null));
                }
            }

            ShowSign(m);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && Owner != null)
            {
                if (Owner.AreThereAvailableVendorsFor(from))
                {
                    list.Add(new VendorsEntry(this));
                }

                if (Owner.VendorInventories.Count > 0)
                {
                    list.Add(new ReclaimVendorInventoryEntry(this));
                }
            }
        }

        private class VendorsEntry : ContextMenuEntry
        {
            private readonly HouseSign m_Sign;

            public VendorsEntry(HouseSign sign)
                : base(6211)
            {
                m_Sign = sign;
            }

            public override void OnClick()
            {
                Mobile from = Owner.From;

                if (!from.CheckAlive() || m_Sign.Deleted || m_Sign.Owner == null || !m_Sign.Owner.AreThereAvailableVendorsFor(from))
                {
                    return;
                }

                if (from.Map != m_Sign.Map || !from.InRange(m_Sign, 5))
                {
                    from.SendLocalizedMessage(1062429); // You must be within five paces of the house sign to use this option.
                }
                else
                {
                    from.SendGump(new HouseGump(HouseGumpPage.Vendors, from, m_Sign.Owner));
                }
            }
        }

        private class ReclaimVendorInventoryEntry : ContextMenuEntry
        {
            private readonly HouseSign m_Sign;

            public ReclaimVendorInventoryEntry(HouseSign sign)
                : base(6213)
            {
                m_Sign = sign;
            }

            public override void OnClick()
            {
                Mobile from = Owner.From;

                if (m_Sign.Deleted || m_Sign.Owner == null || m_Sign.Owner.VendorInventories.Count == 0 || !from.CheckAlive())
                {
                    return;
                }

                if (from.Map != m_Sign.Map || !from.InRange(m_Sign, 5))
                {
                    from.SendLocalizedMessage(1062429); // You must be within five paces of the house sign to use this option.
                }
                else
                {
                    from.CloseGump(typeof(VendorInventoryGump));
                    from.SendGump(new VendorInventoryGump(m_Sign.Owner, from));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            _Owner = reader.ReadItem() as BaseHouse;
        }
    }
}
