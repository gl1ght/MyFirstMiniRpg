
abstract class Enemy : Entity
{
    public int ExpReward { get; protected set; }
    public int MoneyReward { get; protected set; }
    public string LootReward { get; protected set; }
    public string descriptionOfEnemy = "Любое существо, которое может вступить в бой с игроком. Каждый противник обладает своими характеристиками, наградой и шансом выпадения добычи.";

    public Enemy(int level,int maxHealth, int damage,string name, string description, int expReward, int moneyReward, string lootReward)
        : base(level,maxHealth, damage, name, description)
    {
        ExpReward = expReward*level;
        MoneyReward = moneyReward;
        LootReward = lootReward;

    }
public void GenerateLevel(int playerLevel, Random dice)
{
    int min = Math.Max(1, playerLevel - 1);
    int max = playerLevel + 1;

    level = dice.Next(min, max + 1);

    UpdateLvlStats();
}


}