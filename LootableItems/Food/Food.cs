
class Food : LootableItems, IUsable
{
    protected int hungerbond;
    public int healbond;
    public string descriptionOfFood = "Восстанавливает запас сытости. Некоторые продукты также могут немного восстановить здоровье.";
    public Food(bool canStack, string name, string description, int standartPrice, float weight,int hungerbond, int healbond) : base(canStack, name, description, standartPrice, weight)
    {
        //Имя, описание, базовая цена, вес, насыщение, лечение
        this.hungerbond = hungerbond;
        this.healbond = healbond;
    }
    public void Use(Player player)
    {
        player.Heal(healbond, true);
        player.Hung(hungerbond);
    }

    public override void ShowItemInfo()
    {
        base.ShowItemInfo();
        System.Console.WriteLine(@$"Востановление голода: {hungerbond}
Востановление здоровья {healbond}%");
    Menu.Bet();
    }

    public override void ShowInfoItem(LootableItems item)
    {
        base.ShowInfoItem(item);
        if (item is Food food)
        {
            Console.WriteLine($"Востановление голода: {food.hungerbond}");
            Console.WriteLine($"Востановление здоровья: {food.healbond}%");
        }
    }
}