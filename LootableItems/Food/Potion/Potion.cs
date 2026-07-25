
abstract class Potion : Food
{
    protected int duration;
    public string descriptionOfPotion = "Восстанавливает запас сытости. Некоторые продукты также могут немного восстановить здоровье. Также может давать временные эффекты.";
    public Potion(bool canStack, string name, string description, int standartPrice, float weight,int hungerbond, int healbond, int duration) : base(canStack, name, description, standartPrice, weight, hungerbond, healbond)
    {
        //Имя, описание, базовая цена, вес, насыщение, лечение
        this.duration = duration;
    }
    public abstract void Reduce(Player player);


}