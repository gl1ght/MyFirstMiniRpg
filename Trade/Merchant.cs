
class Merchant
{
    public List<ShopItem> Items { get; } = new();
    public int TradeMoney {get; set;}
    private Random random = new();
    private int lastRefreshDay = -1;

public void CheckRefresh()
    {
        if (lastRefreshDay == Game.CurrentDay)
            return;

        RefreshShop();

        lastRefreshDay = Game.CurrentDay;
    }

     public void RefreshShop()
    {
        Items.Clear();

        GenerateMaterials();
        GenerateFood();
        GeneratePotions();


            GenerateArmor();
            GenerateWeapons();
            GenerateBackpacks();
            TradeMoney = random.Next(100, 201);
        
    }

    public void ShowShop()
    {
        System.Console.WriteLine("======Лавка торговца======");
        System.Console.WriteLine($"Баланс торговца:{TradeMoney} шекелей");
        System.Console.WriteLine("Доступные предметы:");
        for (int i = 0; i < Items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Items[i].Item.Name} x{Items[i].Count} по цене {Items[i].PriceToBuy} шекелей");
        }
        Menu.Bet();
    }


    public void RemoveItem(ShopItem slot, int amount)
    {
    slot.RemoveOne(amount);

    if (slot.Count == 0)
    {
        Items.Remove(slot);
    }

} 

private void GenerateFood()
{
    int amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Яблоко"),
        amount));

    amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Ягода"),
        amount));

    amount = random.Next(1, 10);

    Items.Add(new ShopItem(
        ItemFabric.Create("Гриб"),
        amount));
}


private void GenerateMaterials()
{
    Items.Add(new ShopItem(
        ItemFabric.Create("Бревно"),
        random.Next(3, 12)));

    Items.Add(new ShopItem(
        ItemFabric.Create("Камень"),
        random.Next(3, 8)));
}

private void GeneratePotions()
    {
    if(random.Next(100) < 40)
        {    
        Items.Add(new ShopItem(
        ItemFabric.Create("Зелье исцеления"),
        random.Next(1, 4)));
        }
    if(random.Next(100) < 40)
    {    
    Items.Add(new ShopItem(
        ItemFabric.Create("Зелье насыщения"),
        random.Next(1, 4)));
    }
    }

private void GenerateWeapons()
{
    if (random.Next(100) < 10)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Деревяный меч"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Каменный меч"),
            1));
    }
}


private void GenerateArmor()
{
    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаный шлем"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаный нагрудник"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаные поножи"),
            1));
    }

    if (random.Next(100) < 5)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Кожаные сапоги"),
            1));
    }
}


private void GenerateBackpacks()
{
    if (random.Next(100) < 10)
    {
        Items.Add(new ShopItem(
            ItemFabric.Create("Маленький рюкзак"),
            1));
    }
}

public void ShopInteract(Player player)
    {
        while (true)  
        {
        ShowShop();
        System.Console.WriteLine($"Ваш баланс: {player.money} шекелей");
        System.Console.WriteLine("Что желаете сделать?");
        System.Console.WriteLine("0 - вернуться");
        System.Console.WriteLine("1 - купить");
        System.Console.WriteLine("2 - продать");
         switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                BuyItem(player);
                break;
            case ConsoleKey.D2:
                SellItem(player);
                break;
            case ConsoleKey.D0:
                return;
            default:
                System.Console.WriteLine("Выберите что то из меню");
                break;

        }
        }
    }

private void BuyItem(Player player)
    {
        try{
        System.Console.WriteLine("Выберите предмет который хотите купить:");
        int choice = Convert.ToInt32(Console.ReadLine());
        ShopItem slot = GetSlot(choice - 1);
        if (slot == null)
        {
        Console.WriteLine("Такого предмета нет.");
        return;
        }
        Menu.Bet();
        System.Console.WriteLine($"{slot.Item.Name} x {slot.Count}");
        System.Console.WriteLine("Введите количество которое хотите купить:");
        choice = Convert.ToInt32(Console.ReadLine());
        if (choice <= 0)
        {
            Console.WriteLine("Введите число больше нуля.");
            return;
        }
        if(choice > slot.Count)
            {
                System.Console.WriteLine("У торговца недостаточно предметов");
                return;
            }
        int totalPrice = slot.PriceToBuy * choice;
        System.Console.WriteLine($"Итоговая цена:{totalPrice}");
        if(player.money < totalPrice)
            {
                System.Console.WriteLine("У вас недостаточно средств");
                return;
            }
        bool purchasePending = true;
        while (purchasePending)
        {
        System.Console.WriteLine("Подтвердить покупку?");
        bool confirmation = Confirmation();
        if(confirmation == false)
            {
                System.Console.WriteLine("Покупка отменена");
                purchasePending = false;
            }
        else if(confirmation == true)
        {
        RemoveItem(slot, choice);
        player.Inventory.AddItem(slot.Item, choice);
        player.Mony(-totalPrice);
        TradeMoney += totalPrice;
        purchasePending = false;
        System.Console.WriteLine("Покупка прошла успешно!");
        }
        }
        }
        catch (System.FormatException)
        {
            System.Console.WriteLine("Выбраного предмета несуществует");
            return;
        }
    }


private void SellItem(Player player)
    {
        System.Console.WriteLine("Выберите какой предмет хотите продать:");
        player.Inventory.Show();
        try
        {
            int choice = Convert.ToInt32(Console.ReadLine());
            InventorySlot slot = player.Inventory.GetSlot(choice - 1);
            if (slot == null)
            {
                Console.WriteLine("Такого предмета нет.");
                return;
            }
            Menu.Bet();
            Console.WriteLine($"{slot.Item.Name} x {slot.Count}");
            ShopItem itemToSell = new ShopItem(slot.Item ,slot.Count);
            System.Console.WriteLine($"Торговец предлагает за 1 этот предмет:{itemToSell.PriceToSell}");
            System.Console.WriteLine("Устраивает ли вас такая цена?");
            bool confirmation = Confirmation();
            if(confirmation){
            System.Console.WriteLine($"Баланс торговца:{TradeMoney}");
            System.Console.WriteLine("Введите количество которое хотите продать");
            choice = Convert.ToInt32(Console.ReadLine());
            if(choice > itemToSell.Count)
                {
                    System.Console.WriteLine("У вас недостаточно предметов");
                    return;
                }

            int finalPrice = itemToSell.PriceToSell*choice;

            if(finalPrice > TradeMoney)
                {
                    System.Console.WriteLine("У торговца недостаточно денег");
                    return;
                }
            System.Console.WriteLine("По итогу этой сделки вы:");
            System.Console.WriteLine($"Отдаете: {itemToSell.Item.Name} x {choice}");
            System.Console.WriteLine($"Получаете: {finalPrice} шекелей");
            System.Console.WriteLine("Подтверждаете сделку?");
            confirmation = Confirmation();
            if(confirmation == true)
                {
                    player.Inventory.RemoveItem(slot, choice);
                    AddItem(slot.Item, choice);
                    TradeMoney -= finalPrice;
                    player.Mony(finalPrice);
                    System.Console.WriteLine("Продажа прошла успешно!");

                }
            }
            
        }
        catch (System.FormatException)
        {
            System.Console.WriteLine("Выбраного предмета несуществует");
            return;
        }
    }

public ShopItem GetSlot(int index)
    {
    if (index < 0 || index >= Items.Count)
        return null;

    return Items[index];
    }

public bool Confirmation()
    {
        while (true)
        {
        System.Console.WriteLine("Да");
        System.Console.WriteLine("Нет");
        string choiceStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(choiceStr))
        {
            Console.WriteLine("Введите Да или Нет.");
            continue;
        }
        string choiceLower = choiceStr.ToLower();
        if(choiceLower == "да"){return true;}
        else if(choiceLower == "нет"){return false;}
        else{System.Console.WriteLine("Принимаються только да или нет");}
        }
    }

public void AddItem(LootableItems item, int count)
{
    ShopItem slot = Items.FirstOrDefault(x => x.Item.GetType() == item.GetType());

    if (slot != null)
    {
        slot.AddOne(count);
        return;
    }

    Items.Add(new ShopItem(item, count));
}


}






class ShopItem
{
    public ShopItem(LootableItems item, int count)
    {
        Item = item;
        Count = count;

    }
    public LootableItems Item { get; set; }

    public int PriceToBuy => Item.StandartPrice;
    public double SellCommision => Item.StandartPrice * 0.2;
    public int PriceToSell => Item.StandartPrice - (int)SellCommision;

    public int Count { get; set; }

    public void AddOne(int amount)
    {
        Count += amount;
    }

    public void RemoveOne(int amount)
    {
        Count -= amount;
    }
    public void RemoveOne()
    {
        Count-- ;
    }
}