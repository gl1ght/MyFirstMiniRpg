
abstract class Potion : Food
{
    private int duration;
    public string description = "Создано опытными алхимиками. Может обладать самыми разными эффектами.";
    public Potion(string name, string description, int standartPrice, float weight,int hungerbond, int healbond, int duration) : base(name, description, standartPrice, weight, hungerbond, healbond)
    {
        //Имя, описание, базовая цена, вес, насыщение, лечение
        this.duration = duration;
    }
    public abstract void Reduce(Player player);
}