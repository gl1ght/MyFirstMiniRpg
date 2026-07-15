
class ItemFabric
{

    public static LootableItems Create(string name)
    {
        switch(name)
        {
            case "Мясо волка":
                return new WolfMeat();

            case "Яблоко":
                return new Apple();

            case "Мясо медведя":
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