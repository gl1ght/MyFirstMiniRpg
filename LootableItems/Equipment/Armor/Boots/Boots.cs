
abstract class Boots : Armor
{
    public Boots(bool canStack, string name, string description, int standartPrice, float weight, int armorProtection, int requiredLevel, int durability) : base(canStack, name, description, standartPrice, weight, armorProtection, requiredLevel, durability)
    {
    }

    public override EquipmentSlot Slot => EquipmentSlot.Feet;
}