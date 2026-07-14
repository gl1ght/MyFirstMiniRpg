
using System.Net.ServerSentEvents;

abstract class LootableItems
{
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



}