
class Combat
{
    private static int Attack(Random dice, Entity entity)
    {
        int a = entity.Damage;
        int b = entity.Damage/2;
        int attack = dice.Next(b,a);
        System.Console.WriteLine($"-{attack}hp!");
        return attack;
    }
    public static void Fight(Player player, Enemy enemy)
    {
        Random dice = new Random();
        enemy.UpdateLvlStats();
        System.Console.WriteLine($"На тебя напал {enemy.name}");
        while (player.isAlive && enemy.isAlive)
        {
            
        if(player.isAlive){
            Thread.Sleep(1000);
            bool endFight = PlayerTurn(player, enemy);
            if (endFight)
                {
                    return;
                }
        if (enemy.isAlive)
            {
                EnemyTurn(player, enemy);    
            }
        }
        }
        if (player.isAlive)
        {
            player.Expi(enemy.ExpReward);
            player.Mony(enemy.MoneyReward);
            System.Console.WriteLine($"Победа!\n+{enemy.ExpReward} опыта, +{enemy.MoneyReward} денег");
            LootableItems reward = ItemFabric.Create(enemy.LootReward);
            player.Inventory.AddItem(reward, 2);
            Thread.Sleep(1000);
        }
        
        
    }

    public static bool PlayerTurn(Player player, Enemy enemy)
    {
        Random dice = new Random();
        bool endTurn = false;
        System.Console.WriteLine($"Ход {player.name} {player.level}-го уровня");
    while (!endTurn)
    {
        Console.WriteLine("1 - Атаковать");
        Console.WriteLine("2 - Инвентарь");
        Console.WriteLine("3 - Побег");
        Menu.Bet();

        switch(Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:

                int attackP = Attack(dice, player);
                enemy.TakeDamage(attackP);
                endTurn = true;
                break;

            case ConsoleKey.D2:

                

                endTurn = player.Inventory.CombatUseChoise(player);;
                break;

            case ConsoleKey.D3:

                bool escape = TryEscape(player, enemy, dice);
                    if (escape)
                    {
                        System.Console.WriteLine("Вам успешно удалось сбежать!");
                        return true;
                    }
                    if (!escape)
                    {
                        System.Console.WriteLine("Вам не удалось сбежать!");
                        int attackE = Attack(dice, enemy);
                        System.Console.WriteLine($"Штрафной урон от врага!");
                        player.TakeDamage(attackE);
                        
                        endTurn = true;
                    }
                break;
        }
        }
    System.Console.WriteLine($"Твое здоровье: {player.Health}\nЗдоровье врага: {enemy.Health}");
            enemy.AliveCheckByHP();
            Menu.Bet();
            return false;
    }

    public static void EnemyTurn(Player player, Enemy enemy)
    {
        Random dice = new Random();
        Menu.Bet();
            System.Console.WriteLine($"Ход {enemy.name} {enemy.level}-го уровня");
            System.Console.WriteLine($"{enemy.name} {enemy.level}-го уровня атакует");
            int attackE = Attack(dice, enemy);
            player.TakeDamage(attackE);
            System.Console.WriteLine($"Твое здоровье: {player.Health}\nЗдоровье врага: {enemy.Health}");
            Menu.Bet();
            player.AliveCheckByHP();
    }

    public static bool TryEscape(Player player, Enemy enemy, Random random)
{
    int chance = 50 + (player.level - enemy.level) * 10;
    System.Console.WriteLine($"Шанс побега составляет {chance}%");
    chance = Math.Clamp(chance, 10, 90);

    return random.Next(1, 101) <= chance;
}
    
}

