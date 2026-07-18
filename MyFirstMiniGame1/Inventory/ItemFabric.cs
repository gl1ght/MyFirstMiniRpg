
class ItemFabric
{

    public static LootableItems Create(string name)
    {
        switch(name)
        {
            case "М'ясо вовка":
                return new WolfMeat();

            case "Яблуко":
                return new Apple();

            case "М'ясо ведмедя":
                return new BearMeat();
            case "Ягода":
                return new Berry();
            case "Гриб":
                return new Mushroom();
            

            default:
                return null;
        }
    }
}