using System;
class Menu
{
    public static void ShowMenu(Player player)
        {
            Bet();
            System.Console.WriteLine("Инструкция:\n1 - искать еду\n2 - идти на работу\n3 - разбить лагерь и отдохнуть\n4 - проверить инвентарь\n5 - статистика\nesc - выйти в меню");
            System.Console.WriteLine($"Уровень:{player.level}\nПрогресс до след уровня {player.Exp}/{player.level*100}\nЗдоровье:{player.Health}\nЕда:{player.Hunger}\nДеньги:{player.money}\nБазовый урон:{player.Damage}");
            Bet();
        }

    public static bool GameLeave()
    {
        Bet();
        System.Console.WriteLine("Выходим...");
        Bet();
        Environment.Exit(0);
        return(false);
    }

    public static void StatsCheck(Player player)
    {
        Bet();
        System.Console.WriteLine($"Уровень:{player.level}\nДней прожито:{player.statDayAlive}\nДней отработано:{player.statDayWork}\nДней в поиске еды:{player.statDayEat}\nДней отдыха:{player.statDaySleep}");
        Bet();
    }

     public static void StartDay(Player player)
    {
        player.NewDay();
        
    }

    public static void Bet()
    {
        System.Console.WriteLine("===============================");
    }

    public static Player StartMenu(SaveData data)
    {
        Player player = Player.CreateNew();
        bool gameNotReady = true;
        while(gameNotReady){
        System.Console.WriteLine("Выберите опцию:\n1 - начать новую игру\n2 - загрузить игру\n3 - miniwiki\nesc - выйти");
        ConsoleKeyInfo option = System.Console.ReadKey(true);
            switch (option.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    
                    gameNotReady = false;
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    player = SaveManager.Load(data, player);
                    gameNotReady = false;
                    break;
  

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    MiniWiki.MiniWikiMenu();
                    break;
                case ConsoleKey.Escape:
                    GameLeave();
                    break;
                default:
                    break;
            }
        
        
        }
        return player;
    }

    public static void SaveChoice(Player player)
    {

        System.Console.WriteLine("Выберите способ сохранения:\n1 - сохранить вручную\n2 - быстрое сохранение");
        ConsoleKeyInfo option = System.Console.ReadKey(true);


        switch (option.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                SaveManager.Save(player);
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                SaveManager.QuickSave(player);
                break;
            default:
                System.Console.WriteLine("Выберите что то из меню");
                break;
        }
    }

    public static Player InGameMenu(Player currentplayer, SaveData data)
    {
        bool inGameMenu = true;
        while(inGameMenu)
        {
            System.Console.WriteLine("Выберите опцию:\n1 - продолжить\n2 - сохранить\n3 - загрузить\n4 - выйти в главное меню");
            ConsoleKeyInfo option = System.Console.ReadKey(true);
            switch (option.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return currentplayer;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Menu.SaveChoice(currentplayer);
                    return currentplayer;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    inGameMenu = false;
                    Player newLoadedPlayer = SaveManager.Load(data, currentplayer);
                    return newLoadedPlayer;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Player newPlayer = Menu.StartMenu(data);
                    return newPlayer;
                default:
                    System.Console.WriteLine("Выберите что то из меню");
                    break;
                
            }          
        }
        return null;
    }

}


