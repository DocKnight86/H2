using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles.MannequinProperty
{
    public abstract class ExtendedWeaponAttr : ValuedProperty
    {
        public override bool IsMagical => true;
        public abstract ExtendedWeaponAttribute Attribute { get; }

        public double GetPropertyValue(Item item)
        {
            return item is BaseWeapon weapon ? weapon.ExtendedWeaponAttributes[Attribute] : 0;
        }

        public override bool Matches(Item item)
        {
            double total = GetPropertyValue(item);

            if (!IsBoolen)
                Value = total;

            if (total != 0)
                return true;

            return false;
        }

        public override bool Matches(List<Item> items)
        {
            double total = 0;

            items.ForEach(x => total += GetPropertyValue(x));

            if (!IsBoolen)
                Value = total;

            if (total != 0)
                return true;

            return false;
        }
    }

    public class AssassinHonedProperty : ExtendedWeaponAttr
    {
        public override Catalog Catalog => Catalog.HitEffects;
        public override bool IsBoolen => true;
        public override int LabelNumber => 1152206;  // Assassin Honed
        public override int Description => 1152383;  // This property provides additional bonus damage when the attacker is facing the same direction as the target.  The bonus damage is based on the weapons original swing speed.  Ranged weapons have a 50% chance to proc.  The maximum bonus is 73% while minimum bonus 13%.  This property can be found on most weapon types.
        public override ExtendedWeaponAttribute Attribute => ExtendedWeaponAttribute.AssassinHoned;
        public override int Hue => 0x43FF;
        public override int SpriteW => 0;
        public override int SpriteH => 330;
    }

    public class FocusProperty : ExtendedWeaponAttr
    {
        public override Catalog Catalog => Catalog.HitEffects;
        public override bool IsBoolen => true;
        public override int LabelNumber => 1150018;  // Focus
        public override int Description => 1152465;  // This property provides increased weapon damage with each successful hit to the same target.  The damage modified by this property cycles from a negative value to a positive value.  The initial damage inflicted by this property is -40% of the items base damage; however the latter damage inflicted by this property is +20% of the items base damage.  When the user switches to a new target the damage increase resets back to the negative value and the cycling begins again.
        public override ExtendedWeaponAttribute Attribute => ExtendedWeaponAttribute.Focus;
        public override int Hue => 0x43FF;
        public override int SpriteW => 270;
        public override int SpriteH => 240;
    }
}
