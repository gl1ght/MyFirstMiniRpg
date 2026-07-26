
class ItemFabric
{

    public static LootableItems Create(string name)
    {
        switch(name)
        {
            case null:
                System.Console.WriteLine("Ошибка:Предмет не найден");
                return null;
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
            case "Зелье исцеления":
                return new HealPotion();
            case "Зелье насыщения":
                return new HungerPotion();
            case "Зелье берсерка":
                return new CombatPotion();
            case "Бревно":
                return new Wood();
            case "Камень":
                return new Stone();
            case "Деревянный меч":
                return new WoodenSword();
            case "Каменный меч":
                return new StoneSword();
            case "Кожаный шлем":
                return new LeatherHelmet();
            case "Кожаные поножи":
                return new LeatherLegs();
            case "Кожаные сапоги":
                return new LeatherBoots();
            case "Кожаный нагрудник":
                return new LeatherChest();
            case "Маленький рюкзак":
                return new SmallBackpack();


            default:
                System.Console.WriteLine("Ошибка:Предмет не найден");
                return null;
        }
    }
}