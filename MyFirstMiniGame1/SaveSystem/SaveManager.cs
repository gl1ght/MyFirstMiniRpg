
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

class SaveManager
{
   
    public static void Save(Player player)
    {
        string saveFolder = "Saves";

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        System.Console.WriteLine("Введите название файла в который хотите сохранить:");
        string fileNameRead = Console.ReadLine();
        string fileName = fileNameRead;
        
        string path = Path.Combine(saveFolder, fileName + ".txt");


 using (StreamWriter writer = new StreamWriter(path))
{
    writer.WriteLine($"level={player.level}");
    writer.WriteLine($"exp={player.Exp}");
    writer.WriteLine($"health={player.Health}");
    writer.WriteLine($"hunger={player.Hunger}");
    writer.WriteLine($"money={player.money}");
    writer.WriteLine($"statDayAlive={player.statDayAlive}");
    writer.WriteLine($"statDayWork={player.statDayWork}");
    writer.WriteLine($"statDayEat={player.statDayEat}");
    writer.WriteLine($"statDaySleep={player.statDaySleep}");

    writer.WriteLine("inventory=1");

    player.Inventory.SaveInventory(writer);
}
    
        System.Console.WriteLine("Сохранено!");
    }

    public static Player Load(SaveData data, Player currentPlayer)
{
    string[] files = Directory.GetFiles("Saves");

    Console.WriteLine("Доступные файлы:");

    foreach (string file in files)
        Console.WriteLine(Path.GetFileNameWithoutExtension(file));

    Console.Write("Введите имя файла: ");
    string fileName = Console.ReadLine();

    string path = Path.Combine("Saves", fileName + ".txt");

    if (!File.Exists(path))
    {
        Console.WriteLine("Файл не существует.");
        return currentPlayer;
    }

    int lineCalc = 0;

    try
    {
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                lineCalc++;

                string line = reader.ReadLine();

                string[] parts = line.Split('=');

                string key = parts[0];
                string value = parts[1];

                switch (key)
                {
                    case "level":
                        data.level = Int(value);
                        break;

                    case "exp":
                        data.exp = Int(value);
                        break;

                    case "health":
                        data.Health = Int(value);
                        break;

                    case "hunger":
                        data.Hunger = Int(value);
                        break;

                    case "money":
                        data.money = Int(value);
                        break;

                    case "statDayAlive":
                        data.statDayAlive = Int(value);
                        break;

                    case "statDayWork":
                        data.statDayWork = Int(value);
                        break;

                    case "statDayEat":
                        data.statDayEat = Int(value);
                        break;

                    case "statDaySleep":
                        data.statDaySleep = Int(value);
                        break;

                    case "inventory":

                        Player player = new Player(data);

                        player.Inventory.LoadInventory(reader);

                        Console.WriteLine("Загрузка прошла успешно!");

                        return player;
                }
            }
        }

        Console.WriteLine("В файле отсутствует раздел inventory.");
        return currentPlayer;
    }
    catch (IndexOutOfRangeException)
    {
        Console.WriteLine($"Файл поврежден. Строка {lineCalc}");
        return currentPlayer;
    }
    catch (FormatException)
    {
        Console.WriteLine($"Неверный формат данных. Строка {lineCalc}");
        return currentPlayer;
    }
}

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
    string fileName = $"QuickSave{quickSaveNumber}.txt";
    string path = Path.Combine(saveFolder, fileName);

    if (!File.Exists(path))
    {
        using (StreamWriter writer = new StreamWriter(path))
    {
    writer.WriteLine($"level={player.level}");
    writer.WriteLine($"exp={player.Exp}");
    writer.WriteLine($"health={player.Health}");
    writer.WriteLine($"hunger={player.Hunger}");
    writer.WriteLine($"money={player.money}");
    writer.WriteLine($"statDayAlive={player.statDayAlive}");
    writer.WriteLine($"statDayWork={player.statDayWork}");
    writer.WriteLine($"statDayEat={player.statDayEat}");
    writer.WriteLine($"statDaySleep={player.statDaySleep}");

    writer.WriteLine("inventory=1");

    player.Inventory.SaveInventory(writer);
    }
        break;
    }

    quickSaveNumber++;
    }
    
        System.Console.WriteLine("Сохранено!");
    
    }

    
     

    

    public static int Int(string value)
    {
        int newInt = Convert.ToInt32(value);
        return(newInt);
    }
}