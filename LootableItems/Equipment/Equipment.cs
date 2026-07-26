
abstract class EquipmentItem : LootableItems, IEquipable
{
    public abstract EquipmentSlot Slot { get; }

    public int RequiredLevel { get; protected set; }

    public int Durability { get; protected set; }

    protected EquipmentItem(
        bool canStack,
        string name,
        string description,
        int standartPrice,
        float weight,
        int requiredLevel,
        int durability)
        : base(canStack, name, description, standartPrice, weight)
    {
        RequiredLevel = requiredLevel;
        Durability = durability;
    }
}