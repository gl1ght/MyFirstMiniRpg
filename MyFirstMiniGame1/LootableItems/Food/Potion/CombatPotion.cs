
class CombatPotion : Potion

{
    public override bool CanUseInCombat => true;
    private int combatPotionDamegaBuff = 25;
    public CombatPotion() : base("Зелье берсерка", "Временно усиливает боевые способности. Использование занимает один ход.", 100, 0.4F, 0, 10, 6)
    {
             //Имя, описание, базовая цена, вес, насыщение, лечение, длительность действия
    }

public override void ShowItemInfo()
    {
        base.ShowItemInfo();
        System.Console.WriteLine(@$"Временный бонус к урону: {combatPotionDamegaBuff}%");
    Menu.Bet();
    }

    public override void Use(Player player)
    {
        int damageBuff = player.Damage * combatPotionDamegaBuff / 100;
        player.AddDamage(damageBuff);
    }
    public override void Reduce(Player player)
    {
       
    }

    public override void ShowInfoItem(LootableItems item)
    {
        base.ShowInfoItem(item);
        if (item is CombatPotion combatPotion)
        {
            Console.WriteLine($"Временный бонус к урону: {combatPotion.combatPotionDamegaBuff}%");
            Console.WriteLine($"Длительность действия: {combatPotion.duration} ходов");
        }
    }
}