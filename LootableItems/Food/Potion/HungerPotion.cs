
class HungerPotion : Potion

{
    public HungerPotion() : base("Зелье насыщения", "Магическим образом утоляет голод, словно после хорошего обеда.", 100, 0.4F, 50, 0, 1)
    {
            //Имя, описание, базовая цена, вес, насыщение, лечение, длительность действия
    }
   public void Reduce()
    {
        
    }

    public override void Reduce(Player player)
    {
        
    }

    public override void ShowInfoItem(LootableItems item)
    {
        base.ShowInfoItem(item);

    }
}