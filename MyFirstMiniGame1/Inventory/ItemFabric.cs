
class ItemFabric
{

    public static LootableItems Create(string name)
    {
        switch(name)
        {
            case "WolfMeat":
                return new WolfMeat();

            case "Apple":
                return new Apple();

            case "BearMeat":
                return new BearMeat();
            case "Berry":
                return new Berry();
            case "Mushroom":
                return new Mushroom();
            

            default:
                return null;
        }
    }
}