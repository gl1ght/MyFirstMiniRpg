
abstract class Weapon : EquipmentItem, IEquipable
{
    public int DamageBuff{get; protected set;}

    public override EquipmentSlot Slot => EquipmentSlot.Weapon;

    public Weapon(bool canStack, string name, string description, int standartPrice, float weight, int damageBuff, int requiredLevel, int durability) : base(canStack, name, description, standartPrice, weight, requiredLevel, durability)
    {
        //стак, Имя, описание, базовая цена, вес,урон уровень прочность
        DamageBuff = damageBuff;
        Category = ItemCategory.Weapon;
    }

}