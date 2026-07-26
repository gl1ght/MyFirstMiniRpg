
class Home : Location
{
    Random dice = new Random();
    public Home() : base("Дом", "Твой уютный небольшой домик. Находиться на отшибе, вдали от городского шума")
    {
    }

    public override void Enter(Player player)
    {
        System.Console.WriteLine("Ты вернулся домой");
    }

    public override void ShowMenu(Player player)
    {
        bool exit = false;
        Enter(player);
        while (!exit)
        {
            player.AliveCheckByHN();
            player.AliveCheckByHP();
        if(player.isAlive){
            bool nextDay = true;           
            Console.WriteLine($"===== {Name} =====");
            Console.WriteLine(Description);
            Menu.MainPlayerStats(player);

            Console.WriteLine("1 - Спать");
            Console.WriteLine("2 - Верстак");
            Console.WriteLine("3 - Проверить инвентарь или снаряжение");
            Console.WriteLine("4 - Сохранение");
            Console.WriteLine("0 - Путешествовать");
            Menu.Bet();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    player.GoToSleep(dice);
                    break;

                case ConsoleKey.D2:
                    System.Console.WriteLine("У тебя дома вообще то нету верстака");
                    nextDay = false;
                    break;

                case ConsoleKey.D3:
                    player.Inventory.GameUseChoice(player);
                    nextDay = false;
                    break;
                case ConsoleKey.D4:
                    Menu.SaveChoice(player);
                    nextDay = false;
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
}