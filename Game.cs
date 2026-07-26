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
        LocationManager locationManager = new LocationManager();
        Random dice = new Random();
        SaveData data = new SaveData();
        Player player = Menu.StartMenu(data);
        player.UpdateLvlStats();
        
        while(gameOnline)
        {
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
                        LocationManager.Show(locationManager, player);
                        nextDay = false;
                        break;
                        
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        locationManager.GoHome(player);
                        nextDay = false;
                        break;
                        
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        player.Inventory.GameUseChoice(player);
                        nextDay = false;
                        break;
                    case ConsoleKey.Escape:
                        player = Menu.InGameMenu(player, data);
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