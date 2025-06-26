using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles.MannequinProperty
{
    public abstract class AbsorptionAttr : ValuedProperty
    {
        public override bool IsMagical => true;
        public abstract SAAbsorptionAttribute Attribute { get; }

        public double GetPropertyValue(Item item)
        {
            if (item is BaseArmor armor)
            {
                return armor.AbsorptionAttributes[Attribute];
            }

            if (item is BaseJewel jewel)
            {
                return jewel.AbsorptionAttributes[Attribute];
            }


            if (item is BaseWeapon weapon)
            {
                return weapon.AbsorptionAttributes[Attribute];
            }

            if (item is BaseClothing clothing)
            {
                return clothing.SAAbsorptionAttributes[Attribute];
            }

            return 0;
        }

        public override bool Matches(Item item)
        {
            Value = GetPropertyValue(item);

            if (Value != 0)
            {
                return true;
            }

            return false;
        }

        public override bool Matches(List<Item> items)
        {
            double total = 0;

            items.ForEach(x => total += GetPropertyValue(x));

            Value = total;

            if (Value != 0)
            {
                return true;
            }

            return false;
        }
    }

    public class DamageEaterProperty : AbsorptionAttr
    {
        public override Catalog Catalog => Catalog.Resistances;
        public override int Order => 6;
        public override bool AlwaysVisible => true;
        public override int LabelNumber => 1154667;  // Damage Eater
        public override int Description => 1152390;  // This property converts a small portion of damage dealt to a player back as health.  The damage inflicted must be the same type of damage as the eater for the property to function.  Similar eater properties stack with other but are capped at 30%.  The "damage all" type of eater is capped at 18%.  The property stores up to 20 healing charges and converts charges every three second from the last time damage was received.  The "damage all" type of damage eater does not stack with specific eaters.  This property is generally only found on armor and shields.
        public override SAAbsorptionAttribute Attribute => SAAbsorptionAttribute.EaterDamage;
        public override int Cap => 30;
        public override int Hue => 0x42FF;
        public override int SpriteW => 270;
        public override int SpriteH => 60;
    }

    public class KineticEaterProperty : AbsorptionAttr
    {
        public override Catalog Catalog => Catalog.Resistances;
        public override int Order => 7;
        public override bool AlwaysVisible => true;
        public override int LabelNumber => 1154666;  // Kinetic Eater
        public override int Description => 1152390;  // This property converts a small portion of damage dealt to a player back as health.  The damage inflicted must be the same type of damage as the eater for the property to function.  Similar eater properties stack with other but are capped at 30%.  The "damage all" type of eater is capped at 18%.  The property stores up to 20 healing charges and converts charges every three second from the last time damage was received.  The "damage all" type of damage eater does not stack with specific eaters.  This property is generally only found on armor and shields.
        public override SAAbsorptionAttribute Attribute => SAAbsorptionAttribute.EaterKinetic;
        public override int Cap => 30;
        public override int Hue => 0x42FF;
        public override int SpriteW => 0;
        public override int SpriteH => 90;
    }

    public class CastingFocusProperty : AbsorptionAttr
    {
        // Only Mobile View
        public override Catalog Catalog => Catalog.Casting;
        public override int LabelNumber => 1116535;  // Casting Focus
        public override int Description => 1152389;  // This property provides a chance to resist any interruptions while casting spells.  It has a cumulative cap of 12%.  The inscription skill can also grant up to a 5% additional bonus which can exceed the item cap.
        public override SAAbsorptionAttribute Attribute => SAAbsorptionAttribute.CastingFocus;
        public override int Cap => 12;
        public override int Hue => 0x1FF;
        public override int SpriteW => 60;
        public override int SpriteH => 210;
    }
}
