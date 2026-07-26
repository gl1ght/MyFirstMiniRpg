
class Forest : Location
{
    public  List<string> Enemies { get; } = new List<string>{"Волк", "Медведь","Бандит"};
    public  List<string> Loot { get; } = new List<string>{"Бревно", "Камень", "Яблоко", "Гриб", "Ягода"};
    public  List<string> RareLoot { get; } = new List<string>{"Маленький рюкзак", "Деревяный меч", "Кожаный шлем", "Кожаный нагрудник", "Кожаные поножи", "Кожаные сапоги"};
    Random dice = new Random();
    public Forest() : base("Лес", "Темнный лес полный загадок и опасностей")
    {
    }

    public override void Enter(Player player)
    {
        System.Console.WriteLine("Вы входите в лес");
    }

     public override void ShowMenu(Player player)
    {
        bool exit = false;
        Enter(player);
        while (!exit)
        {
        if(player.isAlive){
            bool nextDay = true;
            player.AliveCheckByHN();
            player.AliveCheckByHP();
            Console.WriteLine($"===== {Name} =====");
            Console.WriteLine(Description);
            Menu.MainPlayerStats(player);

            Console.WriteLine("1 - Исследовать");
            Console.WriteLine("2 - Рубить деревья");
            Console.WriteLine("3 - Собирать растения");
            Console.WriteLine("4 - Охотиться");
            Console.WriteLine("5 - Разбить лагерь");
            Console.WriteLine("6 - Проверить инвентарь или снаряжение");
            Console.WriteLine("0 - Вернуться");
            Menu.Bet();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    Explore(player);
                    break;

                case ConsoleKey.D2:
                    ChopTrees(player);
                    break;

                case ConsoleKey.D3:
                    GatherPlants(player);
                    break;

                case ConsoleKey.D4:
                    Hunt(player);
                    break;

                case ConsoleKey.D5:
                    Camp(player);
                    break;
                case ConsoleKey.D6:
                    player.Inventory.GameUseChoice(player);
                    break;

                case ConsoleKey.D0:
                    exit = true;
                    nextDay = false;
                    break;
                default:
                    nextDay = false;
                    System.Console.WriteLine("Выберите что то из меню!");
                    break;
            }

            if(nextDay){
                    Game.NextDay(player);
                }
        }
        }
    }

    public void Explore(Player player)
    {
        int gamble = dice.Next(1, 101);
        if(gamble <= 10)
        {
            System.Console.WriteLine("Пока ты исследовал на тебя напали!");
            Enemy enemy = GenerateEnemy();
            Combat.Fight(player, enemy);

        }
        else
        {
      

            System.Console.WriteLine("Исследование прошло успешно!");
            LootableItems item1 = GenerateLoot();
            LootableItems item2 = GenerateLoot();
            int amount1 = dice.Next(1, 3);
            int amount2 = dice.Next(1, 3);
            System.Console.WriteLine($"Тебе удалось найти:\n{item1.Name} x {amount1}\n{item2.Name} x {amount2}");
            player.Inventory.GameAddItem(player, item1, amount1);
            player.Inventory.GameAddItem(player, item2, amount2);
            gamble = dice.Next(1 , 101);
            if (gamble <= 15)
            {
                LootableItems item3 = GenerateRareLoot();
                System.Console.WriteLine("Удача на твоей стороне!");
                System.Console.WriteLine($"Бонусный редкий предмет:\n{item3.Name}");
                player.Inventory.GameAddItem(player, item3);
            }
        }
    }

    public void ChopTrees(Player player)
    {
        System.Console.WriteLine("Ты рубил деревья целый день и смог собрать:");
        LootableItems item = ItemFabric.Create("Бревно");
        int gamble = dice.Next(1, 5);
        System.Console.WriteLine($"{item.Name} x {gamble}");
        player.Inventory.GameAddItem(player, item, gamble);
    }

    public void GatherPlants(Player player)
    {
        System.Console.WriteLine("Ты собирал растения целый день и смог собрать:");
        for(int i = 0; i < 2; i++)
        {
            LootableItems item1 = CategoryGenerateLoot(ItemCategory.Food);
            int gamble1 = dice.Next(1, 5);
            System.Console.WriteLine($"{item1.Name} x {gamble1}");
            player.Inventory.GameAddItem(player, item1, gamble1);
        }
    }

    public void Hunt(Player player)
    {
        int gamble = dice.Next(1, 101);
        if(gamble <= 90)
        {
            System.Console.WriteLine("Ты встретил добычу!");
            Enemy enemy = GenerateEnemy();
            Combat.Fight(player, enemy);
        }
        else
        {
            System.Console.WriteLine("Ты никого не встретил");
        }
        
    }

    public void Camp(Player player)
    {
        player.GoToSleep(dice);
    }

    public Enemy GenerateEnemy()
    {
       int roll = dice.Next(100);

        if (roll < 60)
            return EnemyFabric.Create("Волк");

        if (roll < 80)
            return EnemyFabric.Create("Медведь");

        return EnemyFabric.Create("Бандит");
    }

    public LootableItems GenerateLoot()
    {
        int index = dice.Next(Loot.Count);

        return ItemFabric.Create(Loot[index]);
    
    }

    public LootableItems GenerateRareLoot()
    {
        int index = dice.Next(RareLoot.Count);

        return ItemFabric.Create(RareLoot[index]);
    }

public LootableItems CategoryGenerateLoot(ItemCategory category)
{
    LootableItems item;

    do
    {
        item = GenerateLoot();
    }
    while (item.Category != category);

    return item;
}


}