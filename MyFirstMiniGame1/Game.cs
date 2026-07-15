using System;
using System.Collections.Generic;


class Game
{
    
    

    public static void StartGame(){

        Console.Clear();
        System.Console.WriteLine("Добро пожаловать в симулятор выживания.\nЧтобы продолжить нажми любую клавишу.");
        Console.ReadKey(true);
        bool nextDay = true;
        bool gameOnline = true;
        Random dice = new Random();
        SaveData data = new SaveData();
        Player player = Menu.StartMenu(data);
        player.UpdateLvlStats();

        while(gameOnline)
        {
            player.LevelUp();
            player.AliveCheckByHN();
            player.AliveCheckByHP();
            if(player.isAlive)
            {
            Menu.ShowMenu(player);
            nextDay = true;
                ConsoleKeyInfo task = System.Console.ReadKey(true);
                switch(task.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        player.SearchFood(dice, player);
                        break;
                        
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        player.GoForWork(dice);
                        break;
                        
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        player.GoToSleep(dice);
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        player.Inventory.UseChoice(player);
                        nextDay = false;
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        Menu.StatsCheck(player);
                        nextDay = false;
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        Menu.SaveChoice(player);
                        nextDay = false;
                        break; 
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        player = SaveManager.NewLoad(data, player);
                        nextDay = false;
                        break; 
                    case ConsoleKey.Escape:
                        gameOnline = Menu.GameLeave();
                        nextDay = false;
                        break; 
                    default:
                        System.Console.WriteLine("Выберите что то из инструкции");
                        nextDay = false;
                        break;                                 
                }
                if(nextDay){
                    Menu.StartDay(player);
                }
            }
            else 
            {
                System.Console.WriteLine("Ты проиграл");
                gameOnline = Menu.GameLeave();
            }
        }
}
}