using System.Diagnostics;
using System.Threading;
using Microsoft.VisualBasic;

class Dungeon : Location
{
    Stopwatch sw = new Stopwatch();
    Random dice = new Random();
    public  List<string> Loot { get; } = new List<string>{"Бревно", "Камень", "Яблоко", "Гриб", "Ягода"};
    public  List<string> RareLoot { get; } = new List<string>{"Маленький рюкзак", "Деревяный меч", "Кожаный шлем", "Кожаный нагрудник", "Кожаные поножи", "Кожаные сапоги"};
    List<ConsoleKey> QTEKey = new List<ConsoleKey>{ConsoleKey.W, ConsoleKey.S, ConsoleKey.A, ConsoleKey.D};
    public Dungeon() : base("Подземелье", "Темное и сырое подземелье. Содержит множество опасноестей, однако награда оправдывает риск")
    {
        
    }

    public override void Enter(Player player)
    {
        System.Console.WriteLine("Ты входишь в подземелье");
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

            Console.WriteLine("1 - Исследовать комнату");
            Console.WriteLine("2 - Разбить лагерь и отдохнуть");
            Console.WriteLine("3 - Осмотреть инвентарь или снаряжение");
            Console.WriteLine("0 - Покинуть подземелье");
            Menu.Bet();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    InspectRoom(player);
                    break;

                case ConsoleKey.D2:
                    player.GoToSleep(dice);
                    break;

                case ConsoleKey.D3:
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


   public void InspectRoom(Player player)
    {
        int gamble = dice.Next(1, 101);
        System.Console.WriteLine("Вы входите в комнату");
        if(gamble < 15)//15%
        {
            System.Console.WriteLine("Вы видите только гладкие стены и пол");
            System.Console.WriteLine("На удивление комната оказалась полностью пустой");
        }
        else if(gamble < 25)//10%
        {
            TrapRoom(player);
        }
        else if(gamble < 60)//35%
        {
            CombatRoom(player);
        }
        else if(gamble < 95)//35%
        {
            ChestRoom(player);
        }
        else if(gamble < 100)//5%
        {
            BossRoom(player);
        }
        System.Console.WriteLine("За исследование комнаты ты получаешь опыт(10)!");
        player.Expi(10);
    } 

    private void TrapRoom(Player player)
    {
        System.Console.WriteLine("Вы видите на первый взгляд обычную комнату");
        System.Console.WriteLine("Однако войдя дверь закрываеться и срабатывает ловушка!");
        Thread.Sleep(1000);
        System.Console.WriteLine("Готовся уворачиваться!");
        bool trap = QTE();
        if (trap)
        {
            System.Console.WriteLine("Пронесло!\nТы смог увернуться и остался цел");
        }
        else if (!trap)
        {
            System.Console.WriteLine("Черт!\nТы не смог увернуться и тебя задело");
            int damage = dice.Next(5, 11);
            player.TakeDamage(damage);
        }
        Thread.Sleep(1000);
        System.Console.WriteLine("Ты покидаешь комнату");
    }

    private void CombatRoom(Player player)
    {
    System.Console.WriteLine("Вы видите на первый взгляд обычную комнату");
    System.Console.WriteLine("Однакой войдя на вас нападает монстр!");
    Enemy enemy = GenerateEnemy();
    Combat.Fight(player, enemy);
    
    }

    private void ChestRoom(Player player)
    {
        System.Console.WriteLine("Вы видите на первый взгляд обычную комнату");
        System.Console.WriteLine("Войдя, вы замечаете странный сундук посреди комнаты");
        while (true)
        {
        System.Console.WriteLine("1 - попытаться открыть\n2 - покинуть комнату");
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.D1:
                TryOpenChest(player);
                return;
            case ConsoleKey.D2:
                System.Console.WriteLine("Ты решаешь не трогать сундук и покидаешь комнату");
                return;
            default:
                System.Console.WriteLine("Выбери что то из меню!");
                break;
        }
        }
    }

    private void BossRoom(Player player)
    {
    System.Console.WriteLine("Вы видите очень подозрительную огромную комнату");
    System.Console.WriteLine("Войдя дверь закрываеться и на вас нападает бос!");
    Enemy enemy = GenerateBoss();
    Combat.Fight(player, enemy);
    }

    private bool QTE()
    {
        
         Thread.Sleep(dice.Next(4000, 6001));
        ConsoleKey keyToPress = GetKey(dice.Next(QTEKey.Count));
        System.Console.WriteLine($"ЖМИ {keyToPress}");
        sw.Restart();
        while (sw.ElapsedMilliseconds < 800)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == keyToPress)
                {
                    Console.WriteLine("Успех!");
                    return true;
                }
            }
        }

        Console.WriteLine("Не успел!");
        return false;
    }

    private void TryOpenChest(Player player)
    {
        int gamble = dice.Next(1, 101);
        if(gamble < 50)
        {
            System.Console.WriteLine("Сундук оказался закрыт!");
            while (true)
            {
            System.Console.WriteLine("Попробовать взламать??");
            bool confirmation = Confirmation();
            if (confirmation)
            {
                OpenChestQTE(player);
            }
            else if (!confirmation)
            {
                System.Console.WriteLine("Ты решил не взламывать сундук и покинуть комнату");
                return;
            }
            }

        }
        else if(gamble < 70)
        {
            System.Console.WriteLine("Сундук оказался ловушкой!");
            System.Console.WriteLine("Готовся!");
            bool trap = QTE();
            if (trap)
            {
                System.Console.WriteLine("Пронесло!Тебе удалось увернуться и ты оказался цел");
            }
            else if (!trap)
            {
                System.Console.WriteLine("Черт! Тебе не удалось увернуться и тебя задело");
                int damage = dice.Next(5, 11);
                player.TakeDamage(damage);
            }
        }
        else
        {
            System.Console.WriteLine("Сундук оказался открыт!");
            LootableItems item1 = GenerateLoot();
            LootableItems item2 = GenerateLoot();
            player.Inventory.AddItem(item1, dice.Next(1, 5));
            player.Inventory.AddItem(item2, dice.Next(1, 5));
            if(dice.Next(1, 101) < 50)
            {
                LootableItems item3 = GenerateRareLoot();
                player.Inventory.AddItem(item3);
            }
          

        }
        System.Console.WriteLine("За исследования сундука ты получаешь опыт(10)!");
        player.Expi(10);
    }

    private void OpenChestQTE(Player player)
    {
        System.Console.WriteLine("Готовся!");
        int successProgres = 0;
        for(int i = 0; i < 5; i++)
        {
            bool task = QTE();
            if (task)
            {
                successProgres++;
            }
        }
        if(successProgres >= 3)
        {
            System.Console.WriteLine("Сундук открылся!");
            LootableItems item1 = GenerateLoot();
            LootableItems item2 = GenerateLoot();
            player.Inventory.AddItem(item1, dice.Next(1, 5));
            player.Inventory.AddItem(item2, dice.Next(1, 5));
            if(dice.Next(1, 101) < 50)
            {
                LootableItems item3 = GenerateRareLoot();
                player.Inventory.AddItem(item3);
            }
        }
        else
        {
            System.Console.WriteLine("Неудача");
            return;
        }
    }

    private ConsoleKey GetKey(int index)
    {
        return QTEKey[index];
    }

    private Enemy GenerateEnemy()
    {
        int roll = dice.Next(100);

        if (roll < 60)
            return EnemyFabric.Create("Слизень");

        else return EnemyFabric.Create("Гоблин");

    }

    private Enemy GenerateBoss()
    {
        return EnemyFabric.Create("Гигантский паук");
    }

    private LootableItems GenerateLoot()
    {
        int index = dice.Next(Loot.Count);

        return ItemFabric.Create(Loot[index]);
    
    }

    private LootableItems GenerateRareLoot()
    {
        int index = dice.Next(RareLoot.Count);

        return ItemFabric.Create(RareLoot[index]);
    }

public bool Confirmation()
    {
        while (true)
        {
        System.Console.WriteLine("1 - Да");
        System.Console.WriteLine("2 - Нет");
        ConsoleKey choiceStr = Console.ReadKey(true).Key;
        if(choiceStr == ConsoleKey.D1){return true;}
        else if(choiceStr == ConsoleKey.D2){return false;}
        else{System.Console.WriteLine("Принимаються только 1 или 2");}
        }
    }
}