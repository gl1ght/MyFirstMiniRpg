
class MiniWiki
{
    
    
    
    public static void MiniWikiMenu()
    {
    while (true)
    {
        System.Console.WriteLine("Внимание! Минивики не содержит всех предметов и вещей! Они будут добавлены позже");
        Console.WriteLine("Выберите категорию для просмотра:");
        Console.WriteLine("0. Выход");
        Console.WriteLine("1. Существа");
        Console.WriteLine("2. Предметы");
        ConsoleKeyInfo key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                ShowAllCreatures();
                break;

            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                ShowAllItems();
                break;

            case ConsoleKey.D0:
            case ConsoleKey.NumPad0:
                return;
        }
    }
    }

    public static void ShowAllCreatures()
    {
        Entity entity = null;
        while (true)
        {
        Console.WriteLine("Существа:");
        Console.WriteLine("0. Назад");
        Console.WriteLine("1. Бандит");
        Console.WriteLine("2. Гигантский паук");
        Console.WriteLine("3. Слизень");
        Console.WriteLine("4. Медведь");
        Console.WriteLine("5. Волк");
        Console.WriteLine("6. Гоблин");
        Console.WriteLine("7. Игрок");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                entity = new Bandit();
                entity.ShowInfoEntity();
                break;
            case "2":
                entity = new GiantSpider();
                entity.ShowInfoEntity();
                break;
            case "3":
                entity = new Slime();
                entity.ShowInfoEntity();
                break;
            case "4":
                entity = new Bear();
                entity.ShowInfoEntity();
                break;
            case "5":
                entity = new Wolf();
                entity.ShowInfoEntity();
                break;
            case "6":
                entity = new Goblin();
                entity.ShowInfoEntity();
                break;
            case "7":
                Player wikiplayer = Player.CreateNew();
                entity = wikiplayer;
                entity.ShowInfoEntity();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                ShowAllCreatures();
                break;
        }
    }
    }

    public static void ShowAllItems()
    {
    LootableItems item = null;
    while (true)
    {
        Console.WriteLine("Предметы:");
        Console.WriteLine("0. Назад");
        Console.WriteLine("1. Мясо волка");
        Console.WriteLine("2. Яблоко");
        Console.WriteLine("3. Мясо медведя");
        Console.WriteLine("4. Ягода");
        Console.WriteLine("5. Гриб");
        Console.WriteLine("6. Зелье исцеления");
        Console.WriteLine("7. Зелье насыщения");
        Console.WriteLine("8. Зелье берсерка");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                item = new WolfMeat();
                item.ShowInfoItem(item);
                break;
            case "2":
                item = new Apple();
                item.ShowInfoItem(item);
                break;
            case "3":
                item = new BearMeat();
                item.ShowInfoItem(item);
                break;
            case "4":
                item = new Berry();
                item.ShowInfoItem(item);
                break;
            case "5":
                item = new Mushroom();
                item.ShowInfoItem(item);
                break;
            case "6":
                item = new HealPotion();
                item.ShowInfoItem(item);
                break;
            case "7":
                item = new HungerPotion();
                item.ShowInfoItem(item);
                break;
            case "8":
                item = new CombatPotion();
                item.ShowInfoItem(item);
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                ShowAllItems();
                break;
        }
    }
    }


}
