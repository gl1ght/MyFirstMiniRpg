using System.Xml.Linq;

class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();

   
    public void AddItem(LootableItems item, int amount = 1)
    {

    foreach (InventorySlot slot in slots)
    {
        if (slot.Item.GetType() == item.GetType())
        {
   
            slot.AddOne(amount);

            
            return;
            
        }
    }
    
    slots.Add(new InventorySlot(item, amount));
    }

    public void RemoveItem(InventorySlot slot, int amount)
    {
    slot.RemoveOne(amount);

    if (slot.Count == 0)
        slots.Remove(slot);

    }
    
    public void UseItem(LootableItems item, Player player)
    {
       foreach (InventorySlot slot in slots)
    {
        if (slot.Item.GetType() == item.GetType())
        {
            slot.Item.Use(player);
            slot.RemoveOne();
            if (slot.Count == 0){
                slots.Remove(slot);
                return;
            }
            
        }
    }
    
        
        
    }

   public void Show()
    {
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

    public void UseChoice(Player player)
    {
        
        bool inventoryactive = true;
        while(inventoryactive)
        {
        try{
        System.Console.WriteLine("Выберите предмет:");
        player.Inventory.Show();
        System.Console.WriteLine("0 - Назад");
        int choice = Convert.ToInt32(Console.ReadLine());
        if(choice == 0)
                {
                    return;
                }
        InventorySlot slot = player.Inventory.GetSlot(choice - 1);

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
        player.Inventory.UseItem(slot.Item, player);
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
        break;
    default:
        System.Console.WriteLine("Выберите что то из списка");
        break;
}
        }
        catch (System.FormatException)
        {
            System.Console.WriteLine("Выбраного предмета несуществует");
        }
    }
        
    }

    public InventorySlot GetSlot(int index)
    {
    if (index < 0 || index >= slots.Count)
        return null;

    return slots[index];
    }

public void LoadFromXml(XElement inventoryElement)
{
    slots.Clear();

    foreach (XElement itemElement in inventoryElement.Elements("Item"))
    {
        string itemName = (string)itemElement.Element("Name");

        int count = (int)itemElement.Element("Count");

        LootableItems item = ItemFabric.Create(itemName);

        AddItem(item, count);
    }
}

    public void SaveInventory(StreamWriter writer)
{
    foreach (InventorySlot slot in slots)
    {
        writer.WriteLine($"{slot.Item.GetType().Name}={slot.Count}");
    }
}

    public void LoadInventory(StreamReader reader)
{
    while (!reader.EndOfStream)
    {
        string line = reader.ReadLine();

        string[] parts = line.Split('=');

        string itemName = parts[0];
        int amount = Convert.ToInt32(parts[1]);

        LootableItems item = ItemFabric.Create(itemName);

        AddItem(item, amount);
    }
}

    public XElement ToXml()
{
    XElement inventory = new XElement("Inventory");

    foreach (InventorySlot slot in slots)
    {
        inventory.Add(
            new XElement("Item",
                new XElement("Name", slot.Item.Name),
                new XElement("Count", slot.Count)
            )
        );
    }

    return inventory;
}
}
