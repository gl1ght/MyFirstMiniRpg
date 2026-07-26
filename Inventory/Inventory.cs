using System.Xml.Linq;

class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<InventorySlot> combatSlots = new List<InventorySlot>();
    private double totalWeight;
    public double TotalWeight
    {
        get
        {
            totalWeight = 0;
            totalWeight = slots.Sum(slot => slot.TotalWeight);
            return totalWeight;
        }
    }
    
    

    public void GameAddItem(Player player, LootableItems item, int amount = 1)
    {

        player.Inventory.AddItem(item, amount);
        System.Console.WriteLine($"В твой инвентарь добавлено: {item.Name} в количестве {amount}");
        player.Inventory.InventoryOverload(player);
    }

    public void AddItem(LootableItems item, int amount = 1)
{
    if (item.CanStack)
    {
        InventorySlot slot = slots.FirstOrDefault(s => s.Item.GetType() == item.GetType());

        if (slot != null)
        {
            slot.AddOne(amount);
            return;
        }

        slots.Add(new InventorySlot(item, amount));
    }
    else
    {
        for (int i = 0; i < amount; i++)
        {
            slots.Add(new InventorySlot(item, 1));
        }
    }
}

    public void RemoveItem(InventorySlot slot, int amount)
    {
    slot.RemoveOne(amount);

    if (slot.Count == 0)
        slots.Remove(slot);

    }
    
    public void UseItem(LootableItems item, Player player)
{
    InventorySlot slot = slots.FirstOrDefault(s => s.Item.GetType() == item.GetType());

    if (slot == null)
    {
        Console.WriteLine("Такого предмета нет.");
        return;
    }

    if (slot.Item is not IUsable usable)
    {
        Console.WriteLine("Этот предмет нельзя использовать.");
        return;
    }

    usable.Use(player);

    slot.RemoveOne();

    if (slot.Count == 0)
    {
        slots.Remove(slot);
    }
}

   public void Show()
    {
        SortByName();
        Console.WriteLine("===== Инвентарь =====");

        if (slots.Count == 0)
        {
            Console.WriteLine("Инвентарь пуст.");
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {slots[i].Item.Name} x{slots[i].Count} {slots[i].TotalWeight}kg");
        }
    }

    public bool UseChoice(Player player)
    {
        

        try{
        System.Console.WriteLine("Выберите предмет:");
        player.Inventory.Show();
        Console.WriteLine($"{player.MaxTotalWeight}kg/{TotalWeight}kg");
        System.Console.WriteLine("0 - Назад");
        int choice = Convert.ToInt32(Console.ReadLine());
        if(choice == 0)
                {
                    return false;
                }
        InventorySlot slot = player.Inventory.GetSlot(choice - 1);

    if (slot == null)
    {
        Console.WriteLine("Такого предмета нет.");
        return true;
    }
    Menu.Bet();
    Console.WriteLine(slot.Item.Name);

    Console.WriteLine("1 - Использовать");
    Console.WriteLine("2 - Выбросить");
    Console.WriteLine("3 - Информация");
    Console.WriteLine("esc - Назад");
    switch(Console.ReadKey(true).Key)
    {
    case ConsoleKey.D1:
        if(slot.Item is IEquipable)
        {
          
            EquipItem(slot, player);
        }
        else if(slot.Item is IUsable)
        {
            player.Inventory.UseItem(slot.Item, player);      
        }
        
        break;

    case ConsoleKey.D2:
        System.Console.WriteLine("Введите количество");
        string amount = Console.ReadLine();
        if (int.TryParse(amount, out int result))
        {
            if(slot.Count >= result){
                player.Inventory.RemoveItem(slot, result);
            }
            else
            {
            System.Console.WriteLine("У вас недостаточно предметов");           
            }
        }
        else
        {
            Console.WriteLine("Неверный формат строки!");
        }
        break;

    case ConsoleKey.D3:
        slot.Item.ShowItemInfo();
        break;
    case ConsoleKey.Escape:
        return false;
    default:
        System.Console.WriteLine("Выберите что то из списка");
        break;
}
        }
        catch (System.FormatException)
        {
            System.Console.WriteLine("Выбраного предмета несуществует");
        }
    
        return true;
    }

    public void GameUseChoice(Player player)
    {
        System.Console.WriteLine("Что хотите проверить?");
        System.Console.WriteLine("1 - Инвентарь");
        System.Console.WriteLine("2 - Экипировку");
        switch(Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                bool inventoryactive = true;
                while (inventoryactive)
                {
                    inventoryactive = UseChoice(player);
                }
                break;
            case ConsoleKey.D2:
                player.Equipment.InteractEquipment(player);
                break;
        }
        
    }

    public InventorySlot GetSlot(int index)
    {
    if (index < 0 || index >= slots.Count)
        return null;

    return slots[index];
    }
 
 public bool CombatInventoryShow()
    {
        this.combatSlots.Clear();


           Console.WriteLine("===== Доступные предметы =====");

        if (slots.Count == 0)
        {
            Console.WriteLine("Доступных предметов нету.");
            Menu.Bet();
            return false;
        }
    

        var combatSlots = slots.Where(slot => slot.Item.CanUseInCombat).ToList();
        this.combatSlots = combatSlots;


        for (int i = 0; i < combatSlots.Count; i++)
        {
                    Console.WriteLine($"{i + 1}. {combatSlots[i].Item.Name} x{combatSlots[i].Count} {combatSlots[i].TotalWeight}kg");            
        }
        if(combatSlots.Count == 0)
        {
            Console.WriteLine("Доступных предметов нету.");
            return false;
        }
        return true;
    }


public bool CombatUseChoise(Player player)
    {
        
        bool inventoryactive = true;
        while(inventoryactive)
        {
        try{
        System.Console.WriteLine("Выберите предмет:");
        player.Inventory.CombatInventoryShow();
        System.Console.WriteLine("0 - Назад");
        int choice = Convert.ToInt32(Console.ReadLine());
        if(choice == 0)
                {
                    return false;
                }
        InventorySlot slot = player.Inventory.CombatGetSlot(choice - 1);

    if (slot == null)
    {
        Console.WriteLine("Такого предмета нет.");
        continue;
    }
    Console.WriteLine(slot.Item.Name);

    Console.WriteLine("1 - Использовать");
    Console.WriteLine("2 - Выбросить");
    Console.WriteLine("3 - Информация");
    Console.WriteLine("esc - Назад");
    switch(Console.ReadKey(true).Key)
    {
    case ConsoleKey.D1:
        player.Inventory.CombatUseItem(slot.Item, player);
        break;

    case ConsoleKey.D2:
        System.Console.WriteLine("Введите количество");
        string amount = Console.ReadLine();
        if (int.TryParse(amount, out int result))
        {
            if(slot.Count >= result){
                player.Inventory.RemoveItem(slot, result);
            }
            else
            {
            System.Console.WriteLine("У вас недостаточно предметов");           
            }
        }
        else
        {
            Console.WriteLine("Неверный формат строки!");
        }
        break;

    case ConsoleKey.D3:
        slot.Item.ShowItemInfo();
        break;
    case ConsoleKey.Escape:
        inventoryactive = false;
        return false;
    default:
        System.Console.WriteLine("Выберите что то из списка");
        break;
}
        }
        catch (System.FormatException)
        {
            System.Console.WriteLine("Выбраного предмета несуществует");
            return false;
        }
    }
    return true;
    }
    

public void CombatUseItem(LootableItems item, Player player)
    {
        foreach (InventorySlot slot in combatSlots)
    {
        if (slot.Item.GetType() == item.GetType())
        {
        if (slot.Item is not IUsable usable)
        {
            Console.WriteLine("Этот предмет нельзя использовать.");
            return;
        }

        usable.Use(player);
            slot.RemoveOne();
            if (slot.Count == 0){
                slots.Remove(slot);
                combatSlots.Remove(slot);
                return;
            }
            
        }
    }
}

    public InventorySlot CombatGetSlot(int index)
    {
    if (index < 0 || index >= combatSlots.Count)
        return null;

    return combatSlots[index];
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

    public List<SaveInventoryData> GetSlotsForSave()
{
    List<SaveInventoryData> result = new();

    foreach (InventorySlot slot in slots)
    {
        result.Add(new SaveInventoryData(slot.Item.Name, slot.Count));
    }

    return result;
}


public void LoadFromSave(List<SaveInventoryData> savedSlots)
{
    slots.Clear();

    foreach (var saved in savedSlots)
    {
        LootableItems item = ItemFabric.Create(saved.ItemName);

        if (item != null)
        {
            AddItem(item, saved.Count);
        }
    }
}


public bool CheckInventoryOverload(Player player)
    {
        if (TotalWeight > player.MaxTotalWeight)
        {
            return true;
        }
        return false;
    }

public void InventoryOverload(Player player)
    {
        bool check = player.Inventory.CheckInventoryOverload(player);
        while(check)
        {
            Menu.Bet();
            System.Console.WriteLine("Ваш инвентарь перегружен!");;
            System.Console.WriteLine("Выбросите какой нибудь предмет");
            player.Inventory.UseChoice(player);
            check = player.Inventory.CheckInventoryOverload(player);
            if (!check)
            {
                System.Console.WriteLine("Перегрузка снята!");
                Console.WriteLine($"{player.MaxTotalWeight}kg/{TotalWeight}kg");
            }
        }
        
       
    }

public void EquipItem(InventorySlot slot, Player player)
{
    bool success = player.Equipment.Equip(slot, player);

    if (!success)
    {
        Console.WriteLine("Этот предмет нельзя экипировать.");
    }
}

public void SortByName()
{
    slots = slots
        .OrderBy(s => s.Item.Name)
        .ToList();
}



}

