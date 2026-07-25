
class HealPotion : Potion

{
    public override bool CanUseInCombat => true;
    public HealPotion() : base("Зелье исцеления", "Восстанавливает здоровье во время боя или путешествия. После использования исчезает.", 100, 0.4F, 0, 50, 1)
    {
             //Имя, описание, базовая цена, вес, насыщение, лечение, длительность действия
    }

    public override void Reduce(Player player)
    {
       
    }
}
