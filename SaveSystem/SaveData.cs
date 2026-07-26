
using System.Collections.Generic;

class SaveData
{
        public int level { get; set; } = 1;
        public int exp { get; set; } = 0;
        public int Health { get; set; } = 100;
        public int Hunger { get; set; } = 100;
        public int money { get; set; } = 1000;
        public int statDayAlive { get; set; } = 0;
        public int statDaySleep { get; set; } = 0;
        public int statDayWork { get; set; } = 0;
        public int statDayEat { get; set; } = 0;

        public List<SaveInventoryData> slots { get; set; } = new List<SaveInventoryData>();

        public string Weapon{ get; set; }
        public string Helmet{ get; set; }
        public string Chest{ get; set; }
        public string Legs{ get; set; }
        public string Boots{ get; set; }
        public string Shield{ get; set; }
        public string Backpack{ get; set; }
}

class SaveInventoryData
{
        public string ItemName { get; set; }
        public int Count { get; set; }
        
        public SaveInventoryData(string itemName, int count)
        {
                ItemName = itemName;
                Count = count;
        }
}





