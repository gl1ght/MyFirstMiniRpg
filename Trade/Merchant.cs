
class Merchant
{
    public List<ShopItem> Items { get; } = new();

    private Random random = new();
    private int lastRefreshDay = -1;

public void CheckRefresh()
    {
        if (lastRefreshDay == Game.CurrentDay)
            return;

        RefreshShop();

        lastRefreshDay = Game.CurrentDay;
    }

     public void RefreshShop()
    {
        Items.Clear();

        GenerateMaterials();
        GenerateFood();



            GenerateArmor();
            GenerateWeapons();
            GenerateBackpacks();
        
    }

    public void ShowShop()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Items[i].Item.Name} x{Items[i].Count} по цене {Items[i].Price} шекелей");
        }
    }

    public void RemoveItem(ShopItem slot, int amount)
    {
    slot.RemoveOne(amount);

    if (slot.Count == 0)
    {
        Items.Remove(slot);
    }

} 

private void GenerateFood()
{
    int amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Яблоко"),
        amount));

    amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Ягода"),
        amount));

    amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Гриб"),
        amount));
}


private void GenerateMaterials()
{
    Items.Add(new ShopItem(
        ItemFabric.Create("Бревно"),
        random.Next(3, 12)));

    Items.Add(new ShopItem(
        ItemFabric.Create("Камень"),
        random.Next(3, 8)));
}


private void GenerateWeapons()
{
    if (random.Next(100) < 10)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Деревяный меч"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Каменный меч"),
            1));
    }
}


private void GenerateArmor()
{
    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаный шлем"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаный нагрудник"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаные поножи"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаные сапоги"),
            1));
    }
}


private void GenerateBackpacks()
{
    if (random.Next(100) < 10)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Маленький рюкзак"),
            1));
    }
}

}





class ShopItem
{
    public ShopItem(LootableItems item, int count)
    {
        Item = item;
        Count = count;

    }
    public LootableItems Item { get; set; }

    public int Price => Item.StandartPrice;

    public int Count { get; set; }

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