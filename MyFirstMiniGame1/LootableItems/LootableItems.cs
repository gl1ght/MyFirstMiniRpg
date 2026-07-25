
using System.Net.ServerSentEvents;

abstract class LootableItems
{
    public virtual bool CanUseInCombat => false;
     public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int StandartPrice { get; protected set; }
    public float Weight { get; protected set; }
    public LootableItems(string name, string description, int standartPrice, float weight)
    {
        Name = name;
        Description = description;
        StandartPrice = standartPrice;
        Weight = weight;
    }
    public abstract void Use(Player player);

    public virtual void ShowItemInfo()
    {
        Menu.Bet();
        System.Console.WriteLine($"Вывод информации о {Name}");
System.Console.WriteLine(@$"Цена: {StandartPrice}
Вес: {Weight}kg
Описание: {Description}");

    }

public virtual void ShowInfoItem(LootableItems item)
    {
        Console.WriteLine($"Имя: {item.Name}");
        Console.WriteLine($"Описание: {item.Description}");
        Console.WriteLine($"Цена: {item.StandartPrice}");
        Console.WriteLine($"Вес: {item.Weight}");
    }

}