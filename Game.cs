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
        player.Inventory.AddItem(new StoneSword(), 3);
        player.Inventory.AddItem(new LeatherBoots(), 3);
        player.Inventory.AddItem(new LeatherChest(), 3);
        player.Inventory.AddItem(new LeatherHelmet(), 3);
        player.Inventory.AddItem(new LeatherLegs(), 3);
        player.Inventory.AddItem(new StoneSword(), 3);
        player.Inventory.AddItem(new SmallBackpack(), 3);
        

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
                        player.Inventory.GameUseChoice(player);
                        nextDay = false;
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        Menu.StatsCheck(player);
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