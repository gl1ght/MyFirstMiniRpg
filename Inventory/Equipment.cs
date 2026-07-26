
class Equipment
{
    public Weapon Weapon { get; private set; }

    public Helmet Helmet { get; private set; }

    public ChestArmor Chest { get; private set; }

    public Boots Boots { get; private set; }

    public Leg Leg { get; private set; }

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

    public void UnequipWeapon2(Player player)
    {
        if (Weapon == null)
            return;

        player.Inventory.AddItem(Weapon, 1);

        Weapon = null;
    }

    public void UnequipHelmet(Player player)
    {
        if (Helmet == null)
            return;

        player.Inventory.AddItem(Helmet, 1);

        Helmet = null;
    }

    public void UnequipChest(Player player)
    {
        if (Chest == null)
            return;

        player.Inventory.AddItem(Chest, 1);

        Chest = null;
    }

    public void UnequipBoots(Player player)
    {
        if (Boots == null)
            return;

        player.Inventory.AddItem(Boots, 1);

        Boots = null;
    }

    private void UnequipBackpack(Player player)
    {
        if (Backpack == null)
            return;

        player.Inventory.AddItem(Backpack, 1);

        Backpack = null;
    }
}