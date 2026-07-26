
class LocationManager
{
    public Forest Forest { get; } = new();
    public Village Village { get; } = new();
    public Home Home{ get; } = new();
    // public Dungeon Dungeon { get; } = new();

    public Location CurrentLocation { get; private set; }

    public LocationManager()
    {
        CurrentLocation = Forest;
    }

    public void GoToForest()
    {
        CurrentLocation = Forest;
    }

    public void GoToVillage()
    {
        CurrentLocation = Village;
    }
    public void GoHome(Player player)
    {
        CurrentLocation = Home;
        Home.ShowMenu(player);
    }

    // public void GoToDungeon()
    // {
    //     CurrentLocation = Dungeon;
    // }


public static void Show(LocationManager manager, Player player)
    {
        Console.Clear();

        Console.WriteLine("===== Карта =====");
        Console.WriteLine("1 - Лес");
        Console.WriteLine("2 - Деревня");
        Console.WriteLine("3 - Данж");
        Console.WriteLine("0 - Назад");

        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                manager.GoToForest();
                manager.Forest.ShowMenu(player);
                break;

            case ConsoleKey.D2:
                manager.GoToVillage();
                manager.Village.ShowMenu(player);
                break;

            // case ConsoleKey.D3:
            //     manager.ChangeLocation(new Dungeon());
            //     break;
        }
    }


}
