
class InventorySlot
{
    public LootableItems Item { get; }
    public int Count { get; private set; }
     public float TotalWeight
    {
        get
        {
            return Item.Weight * Count;
        }
    }

    public InventorySlot(LootableItems item, int amount)
    {
        Item = item;
        Count = amount;
    }

    public void AddOne(int amount)
    {
        Count += amount;
    }

    public void RemoveOne(int amount)
    {
        Count -= amount;
    }
    public void RemoveOne()
    {
        Count-- ;
    }
}