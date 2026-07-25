
class Weapon : LootableItems, IEquipable
{
    public int DamageBuff{get; protected set;}

    public EquipmentSlot Slot => EquipmentSlot.Weapon;

    public Weapon(bool canStack, string name, string description, int standartPrice, float weight, int damageBuff) : base(canStack, name, description, standartPrice, weight)
    {
        //Имя, описание, базовая цена, вес, насыщение, лечение
        DamageBuff = damageBuff;
    }



}