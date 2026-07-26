
class Equipment
{
    public Weapon Weapon { get; private set; }

    public Helmet Helmet { get; private set; }

    public ChestArmor Chest { get; private set; }

    public Boots Boots { get; private set; }

    public Legs Legs { get; private set; }

    public Backpack Backpack {get; private set;}

    public bool Equip(InventorySlot slot, Player player)
    {
        if (slot.Item is not IEquipable equipable)
            return false;

        switch (equipable.Slot)
        {
            case EquipmentSlot.Weapon:

                EquipWeapon((Weapon)slot.Item, player);
                break;

            case EquipmentSlot.Head:

                EquipHelmet((Helmet)slot.Item, player);
                break;

            case EquipmentSlot.Chest:

                EquipChest((ChestArmor)slot.Item, player);
                break;

            case EquipmentSlot.Feet:

                EquipBoots((Boots)slot.Item, player);
                break;
            case EquipmentSlot.Legs:
                EquipLegs((Legs)slot.Item, player);
                break;
            case EquipmentSlot.Backpack:
                EquipBackpack((Backpack)slot.Item, player);
                break;
            
        }

        player.Inventory.RemoveItem(slot, 1);

        return true;
    }

    private void EquipWeapon(Weapon weapon, Player player)
    {
  
        if (Weapon != null)
            player.Inventory.AddItem(Weapon, 1);

        Weapon = weapon;
    }

    private void EquipHelmet(Helmet helmet, Player player)
    {
        if (Helmet != null)
            player.Inventory.AddItem(Helmet, 1);

        Helmet = helmet;
    }

    private void EquipChest(ChestArmor armor, Player player)
    {
        if (Chest != null)
            player.Inventory.AddItem(Chest, 1);

        Chest = armor;
    }

    private void EquipBoots(Boots boots, Player player)
    {
        if (Boots != null)
            player.Inventory.AddItem(Boots, 1);

        Boots = boots;
    }

    private void EquipBackpack(Backpack backpack, Player player)
    {
        if (Backpack != null)
            player.Inventory.AddItem(Backpack, 1);

        Backpack = backpack;
    }

    private void EquipLegs(Legs legs, Player player)
    {
        if (Legs != null)
            player.Inventory.AddItem(Legs, 1);

        Legs = legs;
    }

    public void UnequipWeapon(Player player)
    {
        if (Weapon == null){
            System.Console.WriteLine("У вас в руках нету оружия");
            return;
        }
        player.Inventory.AddItem(Weapon, 1);

        Weapon = null;
    }

    public void UnequipHelmet(Player player)
    {
        if (Helmet == null)
           {
            System.Console.WriteLine("На вас не одет шлем");
            return;
           }

        player.Inventory.AddItem(Helmet, 1);

        Helmet = null;
    }

    public void UnequipChest(Player player)
    {
        if (Chest == null)
            {
                System.Console.WriteLine("На вас не одет нагрудник");
                return;
            }

        player.Inventory.AddItem(Chest, 1);

        Chest = null;
    }

    public void UnequipBoots(Player player)
    {
        if (Boots == null)
        {
            System.Console.WriteLine("На вас не одеты сапоги");
            return;
        }


        player.Inventory.AddItem(Boots, 1);

        Boots = null;
    }

    private void UnequipBackpack(Player player)
    {
        if (Backpack == null){
            System.Console.WriteLine("На вас не одет рюкзак");
            return;
        }
        player.Inventory.AddItem(Backpack, 1);

        Backpack = null;
    }

    private void UnequipLegs(Player player)
        {
            if (Legs == null)
            {
                System.Console.WriteLine("На вас не одеты поножи");
                return;
            }
            player.Inventory.AddItem(Legs, 1);
            Legs = null;
        }

    public void LoadWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }

    public void LoadHelmet(Helmet helmet)
    {
        Helmet = helmet;
    }

    public void LoadChest(ChestArmor armor)
    {
        Chest = armor;
    }

    public void LoadLegs(Legs armor)
    {
        Legs = armor;
    }

    public void LoadBoots(Boots boots)
    {
        Boots = boots;
    }

    public void LoadBackpack(Backpack backpack)
    {
        Backpack = backpack;
    }

    //   public void LoadShield(Shield shield)
    // {
    //     Shield = shield;
    // }


public void ShowEquipment()
{
    Console.WriteLine("===== Экипировка =====");

    Console.WriteLine($"1.Оружие: {Weapon?.Name ?? "Нет"}");
    // Console.WriteLine($"Щит: {Shield?.Name ?? "Нет"}");
    Console.WriteLine($"2.Шлем: {Helmet?.Name ?? "Нет"}");
    Console.WriteLine($"3.Нагрудник: {Chest?.Name ?? "Нет"}");
    Console.WriteLine($"4.Поножи: {Legs?.Name ?? "Нет"}");
    Console.WriteLine($"5.Ботинки: {Boots?.Name ?? "Нет"}");
    Console.WriteLine($"6.Рюкзак: {Backpack?.Name ?? "Нет"}");

}

public void InteractEquipment(Player player)
    {
        while(true)
        {
        ShowEquipment();
        Menu.Bet();
        System.Console.WriteLine("1 - Снять какое-то снаряжение");
        System.Console.WriteLine("0 - Назад");
         switch(Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                EquipmentRemoveChoise(player);
                break;
            case ConsoleKey.D0:
                return;
            default:
                System.Console.WriteLine("Выберите что то из меню");
                break;
        }
        }
        
    }

public void EquipmentRemoveChoise(Player player)
    {
        System.Console.WriteLine("Введите номер снаряжения которое хотите снять");
         switch(Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                UnequipWeapon(player);
                break;
            case ConsoleKey.D2:
                UnequipHelmet(player);
                break;
            case ConsoleKey.D3:
                UnequipChest(player);
                break;
            case ConsoleKey.D4:
                UnequipLegs(player);
                break;
            case ConsoleKey.D5:
                UnequipBoots(player);
                break;
            case ConsoleKey.D6:
                UnequipBackpack(player);
                break;
        }
    }

}