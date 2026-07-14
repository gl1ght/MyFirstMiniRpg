
abstract class Enemy : Entity
{
    public int ExpReward { get; protected set; }
    public int MoneyReward { get; protected set; }


    public Enemy(int level,int maxHealth, int damage,string name, int expReward, int moneyReward)
        : base(level,maxHealth, damage, name)
    {
        ExpReward = expReward*level;
        MoneyReward = moneyReward;

    }
public void GenerateLevel(int playerLevel, Random dice)
{
    int min = Math.Max(1, playerLevel - 1);
    int max = playerLevel + 1;

    level = dice.Next(min, max + 1);

    UpdateLvlStats();
}


}