
using System.Xml.Linq;
using System.Text.Json;

class SaveManager
{



    public static void QuickSave(Player player)
    {

        string saveFolder = "Saves";
        
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        
        int quickSaveNumber = 1;


    while (true)
    {
    string fileName = $"QuickSave{quickSaveNumber}.json";
    string path = Path.Combine(saveFolder, fileName);

    if (!File.Exists(path))
    {
     {
        try{
        string folder = "Saves";

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        SaveData saveData = new SaveData();
        saveData.level = player.level;
        saveData.exp = player.Exp;
        saveData.Health = player.Health;
        saveData.Hunger = player.Hunger;
        saveData.money = player.money;
        saveData.statDayAlive = player.statDayAlive;
        saveData.statDaySleep = player.statDaySleep;
        saveData.statDayWork = player.statDayWork;
        saveData.statDayEat = player.statDayEat;

        saveData.slots = player.Inventory.GetSlotsForSave();
        
        saveData.Weapon = player.Equipment.Weapon?.Name;
        saveData.Helmet = player.Equipment.Helmet?.Name;
        saveData.Chest = player.Equipment.Chest?.Name;
        saveData.Legs = player.Equipment.Legs?.Name;
        saveData.Boots = player.Equipment.Boots?.Name;
        saveData.Backpack = player.Equipment.Backpack?.Name;
        // saveData.Shield = player.Equipment.Shield?.Name;

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonSaver = JsonSerializer.Serialize(saveData, options);

        File.WriteAllText(path, jsonSaver);

        System.Console.WriteLine("Сохранено!");
    }
        catch(System.UnauthorizedAccessException)
        {
            System.Console.WriteLine("Ошибка: не удалось сохранить файл");
            break;
        }
    }
        break;
    }

    quickSaveNumber++;
    }
    
    }

    public static void Save(Player player)
    {
         try{
        string folder = "Saves";

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        System.Console.WriteLine("Введите название файла в который хотите сохранить:");
        string inputfile = Console.ReadLine();
        string path = Path.Combine(folder, inputfile + ".json");

        SaveData saveData = new SaveData();
        saveData.level = player.level;
        saveData.exp = player.Exp;
        saveData.Health = player.Health;
        saveData.Hunger = player.Hunger;
        saveData.money = player.money;
        saveData.statDayAlive = player.statDayAlive;
        saveData.statDaySleep = player.statDaySleep;
        saveData.statDayWork = player.statDayWork;
        saveData.statDayEat = player.statDayEat;

        saveData.slots = player.Inventory.GetSlotsForSave();
        Console.WriteLine(player.Equipment.Weapon.Name);
        saveData.Weapon = player.Equipment.Weapon?.Name;
        saveData.Helmet = player.Equipment.Helmet?.Name;
        saveData.Chest = player.Equipment.Chest?.Name;
        saveData.Legs = player.Equipment.Legs?.Name;
        saveData.Boots = player.Equipment.Boots?.Name;
        saveData.Backpack = player.Equipment.Backpack?.Name;
        // saveData.Shield = player.Equipment.Shield?.Name;
        
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonSaver = JsonSerializer.Serialize(saveData, options);

        File.WriteAllText(path, jsonSaver);

        System.Console.WriteLine("Сохранено!");
    }
        catch(System.UnauthorizedAccessException)
        {
            System.Console.WriteLine("Ошибка: не удалось сохранить файл");
        }
    }

    public static Player Load(SaveData data, Player currentPlayer)
        {
            string[] files = Directory.GetFiles("Saves");

        Console.WriteLine("Доступные файлы:");

        foreach (string file in files)
            Console.WriteLine(Path.GetFileNameWithoutExtension(file));

        Console.Write("Введите имя файла: ");
        string fileName = Console.ReadLine();

        string path = Path.Combine("Saves", fileName + ".json");

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не существует.");
            return currentPlayer;
        }

        string jsonLoader = File.ReadAllText(path);
        SaveData saveData = JsonSerializer.Deserialize<SaveData>(jsonLoader);
        Player player = new Player(saveData);
        player.Inventory.LoadFromSave(saveData.slots);
        if (saveData.Weapon != null)
        {
            player.Equipment.LoadWeapon(
                (Weapon)ItemFabric.Create(saveData.Weapon));
        }

        if (saveData.Helmet != null)
        {
            player.Equipment.LoadHelmet(
                (Helmet)ItemFabric.Create(saveData.Helmet));
        }

        if (saveData.Chest != null)
        {
            player.Equipment.LoadChest(
                (ChestArmor)ItemFabric.Create(saveData.Chest));
        }

        if (saveData.Legs != null)
        {
            player.Equipment.LoadLegs(
                (Legs)ItemFabric.Create(saveData.Legs));
        }

        if (saveData.Boots != null)
        {
            player.Equipment.LoadBoots(
                (Boots)ItemFabric.Create(saveData.Boots));
        }

        if (saveData.Backpack != null)
        {
            player.Equipment.LoadBackpack(
                (Backpack)ItemFabric.Create(saveData.Backpack));
        }
        // if (saveData.Shield != null)
        // {
        //     player.Equipment.LoadShield(
        //         (Shield)ItemFabric.Create(saveData.Shield));
        // }
        Console.WriteLine("Загрузка успешна!");
        return player;
            
        }

    }
