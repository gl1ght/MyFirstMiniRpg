using System;

// Пошаговый бой для 2D-версии: без Thread.Sleep и Console.
// Каждый вызов PlayerAttack() = один раунд (враг бьёт, затем игрок).
class CombatSession
{
    public Player Player { get; }
    public Enemy Enemy { get; }
    public bool Finished { get; private set; }
    public bool PlayerWon { get; private set; }

    private readonly Random dice = new Random();

    public CombatSession(Player player, Enemy enemy)
    {
        Player = player;
        Enemy = enemy;
        Enemy.UpdateLvlStats();
        GameLog.Add($"На тебе напав {enemy.name} {enemy.level}-го рівня!");
    }

    private int AttackValue(Entity entity)
    {
        int high = entity.Damage;
        int low = entity.Damage / 2;
        return dice.Next(low, high + 1);
    }

    // Результат одного раунда — используется UI для эффектов.
    public struct RoundResult
    {
        public int EnemyDamage;    // сколько враг нанёс игроку
        public int PlayerDamage;   // сколько игрок нанёс врагу
        public bool PlayerAttacked; // успел ли игрок ответить
    }

    public RoundResult PlayerAttack()
    {
        var result = new RoundResult();
        if (Finished)
            return result;

        int enemyHit = AttackValue(Enemy);
        Player.TakeDamage(enemyHit);
        result.EnemyDamage = enemyHit;
        GameLog.Add($"{Enemy.name} атакує: -{enemyHit}hp");
        Player.AliveCheckByHP();
        if (!Player.isAlive)
        {
            Finish(false);
            return result;
        }

        int playerHit = AttackValue(Player);
        Enemy.TakeDamage(playerHit);
        result.PlayerDamage = playerHit;
        result.PlayerAttacked = true;
        GameLog.Add($"{Player.name} атакує: -{playerHit}hp");
        Enemy.AliveCheckByHP();
        if (!Enemy.isAlive)
        {
            Finish(true);
        }

        return result;
    }

    private void Finish(bool won)
    {
        Finished = true;
        PlayerWon = won;

        if (won)
        {
            Player.Expi(Enemy.ExpReward);
            Player.Mony(Enemy.MoneyReward);
            GameLog.Add($"Перемога! +{Enemy.ExpReward} досвіду, +{Enemy.MoneyReward} грошей");
        }
        else
        {
            GameLog.Add($"{Enemy.name} виявився сильнішим...");
        }
    }
}
