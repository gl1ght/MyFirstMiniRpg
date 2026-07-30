
class EnemyFabric
{
     public static Enemy Create(string name)
    {
        switch(name)
        {
            case null:
                System.Console.WriteLine("Ошибка:Предмет не найден");
                return null;
            case "Волк":
                return new Wolf();
            case "Медведь":
                return new Bear();
            case "Бандит":
                return new Bandit();
            case "Слизень":
                return new Slime();
            case "Гоблин":
                return new Goblin();
            case "Гигантский паук":
                return new GiantSpider();


            default:
                System.Console.WriteLine("Ошибка:Предмет не найден");
                return null;
        }
}
}