
class MiniWiki
{
    public static void ShowInfoEntity(Entity entity)
    {
        Console.WriteLine($"Имя: {entity.name}");
        Console.WriteLine($"Описание: {entity.description}");
        Console.WriteLine($"Уровень: {entity.level}");
        Console.WriteLine($"Здоровье: {entity.baseHealth}");
        Console.WriteLine($"Урон: {entity.basedamage}");
    }
    public static void ShowInfoItem(LootableItems item)
    {
        Console.WriteLine($"Имя: {item.Name}");
        Console.WriteLine($"Описание: {item.Description}");
        Console.WriteLine($"Цена: {item.StandartPrice}");
        Console.WriteLine($"Вес: {item.Weight}");
    }
    
    public static void MiniWikiMenu()
    {
    while (true)
    {
        Console.WriteLine("Выберите категорию для просмотра:");
        Console.WriteLine("1. Существа");
        Console.WriteLine("2. Предметы");
        Console.WriteLine("3. Выход");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ShowAllCreatures();
                break;
            case "2":
                ShowAllItems();
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                MiniWikiMenu();
                break;
        }
    }
    }

    public static void ShowAllCreatures()
    {
        Console.WriteLine("Существа:");
        Console.WriteLine("1. Бандит");
        Console.WriteLine("2. Гигантский паук");
        Console.WriteLine("3. Слизень");
        Console.WriteLine("4. Медведь");
        Console.WriteLine("5. Назад");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ShowInfoEntity(new Bandit());
                break;
            case "2":
                ShowInfoEntity(new GiantSpider());
                break;
            case "3":
                ShowInfoEntity(new Slime());
                break;
            case "4":
                ShowInfoEntity(new Bear());
                break;
            case "5":
                MiniWikiMenu();
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                ShowAllCreatures();
                break;
        }
    }

    public static void ShowAllItems()
    {
        Console.WriteLine("Предметы:");
        Console.WriteLine("1. Мясо волка");
        Console.WriteLine("2. Яблоко");
        Console.WriteLine("3. Мясо медведя");
        Console.WriteLine("4. Ягода");
        Console.WriteLine("5. Гриб");
        Console.WriteLine("6. Зелье исцеления");
        Console.WriteLine("7. Зелье насыщения");
        Console.WriteLine("8. Зелье берсерка");
        Console.WriteLine("9. Назад");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ShowInfoItem(new WolfMeat());
                break;
            case "2":
                ShowInfoItem(new Apple());
                break;
            case "3":
                ShowInfoItem(new BearMeat());
                break;
            case "4":
                ShowInfoItem(new Berry());
                break;
            case "5":
                ShowInfoItem(new Mushroom());
                break;
            case "6":
                ShowInfoItem(new HealPotion());
                break;
            case "7":
                ShowInfoItem(new HungerPotion());
                break;
            case "8":
                ShowInfoItem(new CombatPotion());
                break;
            case "9":
                MiniWikiMenu();
                return;
            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                ShowAllItems();
                break;
        }
    }



}
