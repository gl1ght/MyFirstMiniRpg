
abstract class Location
{
    public string Name { get; protected set; }
    public string Description {get; protected set;}

    public List<LootableItems> Loot { get; } = new();

    public abstract void Enter(Player player);
    public abstract void ShowMenu(Player player);
    public Location (string name, string description)
    {
        Name = name;
        Description = description;
    }
}