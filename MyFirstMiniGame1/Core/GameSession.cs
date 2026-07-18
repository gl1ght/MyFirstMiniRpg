using System;

// Оркестратор игрового процесса для 2D-версии: держит игрока, счётчик дней,
// активный бой и связывает действия с игровой логикой (без Console).
class GameSession
{
    public Player Player { get; private set; }
    public CombatSession? Combat { get; private set; }

    private readonly Random dice = new Random();
    private Enemy? pendingFoodEnemy; // враг, встреченный при поиске еды (даёт мясо при победе)

    public bool PlayerAlive => Player.isAlive;
    public int DaysAlive => Player.statDayAlive;

    public GameSession(Player player)
    {
        Player = player;
        Player.UpdateLvlStats();
    }

    public void SearchFood()
    {
        Enemy? enemy = Player.SearchFood(dice);
        if (enemy != null)
        {
            enemy.GenerateLevel(Player.level, dice);
            pendingFoodEnemy = enemy;
            Combat = new CombatSession(Player, enemy);
            // день завершится после боя (EndCombat)
        }
        else
        {
            AdvanceDay();
        }
    }

    public void Work()
    {
        Player.GoForWork(dice);
        AdvanceDay();
    }

    public void Sleep()
    {
        Player.GoToSleep(dice);
        AdvanceDay();
    }

    // Начать бой напрямую (например, при столкновении с врагом в мире).
    public void StartCombat(Enemy enemy)
    {
        pendingFoodEnemy = null;
        Combat = new CombatSession(Player, enemy);
    }

    // Втекти з бою: бій скасовується без нагороди.
    public void FleeCombat()
    {
        pendingFoodEnemy = null;
        Combat = null;
    }

    // Вызывается UI, когда бой завершён и игрок нажал "Продолжить".
    public void EndCombat()
    {
        if (Combat == null)
            return;

        if (Combat.PlayerWon)
        {
            if (pendingFoodEnemy is Wolf)
            {
                Player.Inventory.AddItem(new WolfMeat(), 2);
                Player.Hung(100);
                GameLog.Add("Ти здобув м'ясо вовка x2");
                AdvanceDay(); // пошук їжі = новий день
            }
            else if (pendingFoodEnemy is Bear)
            {
                Player.Inventory.AddItem(new BearMeat(), 2);
                Player.Hung(100);
                GameLog.Add("Ти здобув м'ясо ведмедя x2");
                AdvanceDay();
            }
            else
            {
                // бой в мире: награда уже начислена, день не идёт
                Player.LevelUp();
                Player.AliveCheckByHN();
                Player.AliveCheckByHP();
            }
        }

        pendingFoodEnemy = null;
        Combat = null;
    }

    private void AdvanceDay()
    {
        Player.NewDay();      // statDayAlive++, Hung(-10)
        Player.LevelUp();
        Player.AliveCheckByHN();
        Player.AliveCheckByHP();
    }
}
