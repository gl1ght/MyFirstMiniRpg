
abstract class Armor : EquipmentItem
{
    public int ArmorProtection { get; protected set; }

    protected Armor(
        bool canStack,
        string name,
        string description,
        int standartPrice,
        float weight,
        int armorProtection, int requiredLevel, int durability)
        : base(canStack, name, description, standartPrice, weight, requiredLevel, durability)
    {
        ArmorProtection = armorProtection;
    }
    //стак, Имя, описание, базовая цена, вес,урон уровень прочность
}