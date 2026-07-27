
class Village : Location
{
    Random dice = new Random();
    Merchant merchant = new Merchant();

    public Village() : base("Деревня", "Небольшое сельское поселение, жители которого обычно занимаются сельским хозяйством.")
    {
    }

    public override void Enter(Player player)
    {
        System.Console.WriteLine("Ты входишь в деревню");
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

            Console.WriteLine("1 - Лавка торговца");
            Console.WriteLine("2 - Лавка кузнеца");
            Console.WriteLine("3 - Общественная мастерская");
            Console.WriteLine("4 - Таверна");
            Console.WriteLine("5 - Подработка у фермера");
            Console.WriteLine("6 - Проверить инвентарь или снаряжение");
            Console.WriteLine("0 - Вернуться");
            Menu.Bet();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    merchant.CheckRefresh();
                    merchant.ShopInteract(player);
                    nextDay = false;
                    break;

                case ConsoleKey.D2:
                    System.Console.WriteLine("Ты подходишь к лавце кузнеца");
                    System.Console.WriteLine("На входе висит таблица с красной надписью:\"Закрыто, кузнец в отъезде\"");
                    nextDay = false;
                    break;

                case ConsoleKey.D3:
                    System.Console.WriteLine("Ты подходишь к общественной мастерской");
                    System.Console.WriteLine("На входе висит таблица с красной надписью:\"Закрыто, проводиться ремонт\"");
                    nextDay = false;
                    break;

                case ConsoleKey.D4:
                    nextDay = false;
                    break;

                case ConsoleKey.D5:
                    nextDay = false;

                    break;
                case ConsoleKey.D6:
                    player.Inventory.GameUseChoice(player);
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