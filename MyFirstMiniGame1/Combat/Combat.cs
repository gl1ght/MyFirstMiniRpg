
class Combat
{
    private static int Attack(Random dice, Entity entity)
    {
        int a = entity.Damage;
        int b = entity.Damage/2;
        int attack = dice.Next(b,a);
        System.Console.WriteLine($"-{attack}hp");
        return(attack);
    }
    public static void Fight(Player player, Enemy enemy)
    {
        Random dice = new Random();
        enemy.UpdateLvlStats();
        while (player.isAlive && enemy.isAlive)
        {

            Thread.Sleep(1000);
            Menu.Bet();
            System.Console.WriteLine($"{enemy.name} {enemy.level}-го уровня атакует!");
            int attackE = Attack(dice, enemy);
            player.TakeDamage(attackE);
            System.Console.WriteLine($"Твое здоровье: {player.Health}\nЗдоровье врага: {enemy.Health}");
            Menu.Bet();
            player.AliveCheckByHP();
        if(player.isAlive){
            Thread.Sleep(1000);
            System.Console.WriteLine($"{player.name} {player.level}-го уровня атакует!\nНажми любую клавишу чтобы атаковать!");
            Console.ReadKey(true);
            int attackP = Attack(dice, player);
            enemy.TakeDamage(attackP);
            System.Console.WriteLine($"Твое здоровье: {player.Health}\nЗдоровье врага: {enemy.Health}");
            enemy.AliveCheckByHP();
            Menu.Bet();
        }
        }
        if (player.isAlive)
        {
            player.Expi(enemy.ExpReward);
            player.Mony(enemy.MoneyReward);
            System.Console.WriteLine($"Победа!\n+{enemy.ExpReward} опыта, +{enemy.MoneyReward} денег");
            Thread.Sleep(1000);
        }
        
    }
}