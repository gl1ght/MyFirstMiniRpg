
abstract class Backpack : EquipmentItem
{
    public int WeightBuff{get; private set;}
    public Backpack(bool canStack, string name, string description, int standartPrice, float weight, int weightBuff, int requiredLevel, int durability) : base(canStack, name, description, standartPrice, weight,requiredLevel, durability)
    {
        WeightBuff = weightBuff;
    }

    public override EquipmentSlot Slot => EquipmentSlot.Backpack;
}