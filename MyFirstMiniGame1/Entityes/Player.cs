
using System.Data;
using System.Xml.Serialization;
using System.Xml.Linq;

class Player : Entity
{
    
    public Inventory Inventory { get; private set; }
    public int money {get; private set;}= 1000;
    public int hunger{get; private set;} = 100;
    public int statDayAlive {get; private set;}= 0;
    public int statDayWork{get; private set;} = 0;
    public int statDayEat{get; private set;} = 0;
    public int statDaySleep{get; private set;} = 0;
    public int Exp{get; protected set;}
    
    public int Hunger{get{return hunger;} protected set
                {
            if (value > 100)
                hunger = 100;
            else if (value < 0)
                hunger = 0;
            else
                hunger = value;
                }}


    public Player(SaveData data) : base(data.level,100, 20, "Игрок")
    {
        
        level = data.level;
        
       
        Exp = data.exp;
        Hunger = data.Hunger;
        money = data.money;
        statDayAlive = data.statDayAlive;
        statDaySleep = data.statDaySleep;
        statDayWork = data.statDayWork;
        statDayEat = data.statDayEat;
        MaxHealth = data.Health;
        Inventory = new Inventory();
        UpdateLvlStats();
           
    }
    

public static Player CreateNew()
    {
        SaveData data = new SaveData();

        data.Health = 100;
        data.Hunger = 100;
        data.money = 1000;
        data.level = 1;
        data.exp = 0;
        data.statDayAlive = 0;
        data.statDaySleep = 0;
        data.statDayWork = 0;
        data.statDayEat = 0;
  

        return new Player(data);
    }

public void SearchFood(Random dice, Player player)
        {
                    
                    statDayEat++;
                    System.Console.WriteLine("Чтобы искать еду нажми любую клавишу");
                    Console.ReadKey(true);
                    int gamble = dice.Next(1, 11);
                    if(gamble == 1 || gamble == 2)
                    {
                        System.Console.WriteLine("Ты ничего не нашел");
                        
                    }
                    else if(gamble == 3 || gamble == 4)
                    {
                        gamble = dice.Next(1, 6);
                        if(gamble <= 4)
                        {
                        Wolf wolf = new Wolf();
                        SearchFoodFight(dice,player,wolf);
                        player.Inventory.AddItem(new WolfMeat(), 2);
                        }
                        else if(gamble == 5)
                        {
                        Bear bear = new Bear();
                        SearchFoodFight(dice,player,bear);
                        player.Inventory.AddItem(new BearMeat(), 2);
                        }
                    }
                    else
                    {
                        gamble = dice.Next(1, 4);
                        switch (gamble)
                        {
                          case 1:
                          gamble = dice.Next(3, 7);
                          player.Inventory.AddItem(new Mushroom(), gamble);
                          System.Console.WriteLine($"Ты нашел грибы {gamble}");
                          break;  
                          case 2:
                          gamble = dice.Next(3, 7);
                          player.Inventory.AddItem(new Berry(), gamble);
                          System.Console.WriteLine($"Ты нашел ягоды {gamble}");
                          break;  
                          case 3:
                          gamble = dice.Next(3, 7);
                          player.Inventory.AddItem(new Apple(), gamble);
                          System.Console.WriteLine($"Ты нашел яблоки {gamble}");
                          break;  
                        }


                        
                        
                    }
                    player.Hung(-10);
        }

 public void GoForWork(Random dice)
    {
         statDayWork++;
        System.Console.WriteLine("Чтобы работать нажми любую клавишу");
        Console.ReadKey(true);
        int gamble = dice.Next(1, 6);
        if(gamble == 1)
        {
            System.Console.WriteLine("Ты работал слишком плохо и тебе отказались платить");
            Hung(-10);
          
        }
        else if(gamble == 2)
        {
            System.Console.WriteLine("Ты работал слишком хорошо и получил премию");
            Hung(-30);
            money += 400;
         
        }
        else
        {
            System.Console.WriteLine("Ты работал нормально и получил зарплату");
            Hung(-20);
            money += 200;
        }    
    


    }

public void GoToSleep(Random dice)
    {

         statDaySleep++;
                    System.Console.WriteLine("Чтобы отдохнуть нажми любую клавишу");
                    Console.ReadKey(true);
                    int gamble = dice.Next(1, 11);
                    if(gamble == 1)
                    {
                        System.Console.WriteLine("Тебя мучала бесспоница и ты не смог уснуть");
                        Hung(-10);
                        TakeDamage(10);
                        
                    }
                    else
                    {
                        System.Console.WriteLine("Ты хорошо отдохнул");
                        Hung(-10);
                        Heal(50,true);
                    }
    }

    public static Player LoadFromXml(XElement element)
{
    SaveData data = new SaveData();

    data.level = (int)element.Element("level");
    data.exp = (int)element.Element("exp");
    data.Health = (int)element.Element("health");
    data.Hunger = (int)element.Element("hunger");
    data.money = (int)element.Element("money");
    data.statDayAlive = (int)element.Element("statDayAlive");
    data.statDayEat = (int)element.Element("statDayEat");
    data.statDayWork = (int)element.Element("statDayWork");
    data.statDaySleep = (int)element.Element("statDaySleep");

    Player player = new Player(data);

    XElement inventoryElement = element.Element("Inventory");

    player.Inventory.LoadFromXml(inventoryElement);

    return player;
}

public void NewDay()
    {
        statDayAlive++;
        Hung(-10);
    }

public void AliveCheckByHN()
    {
        if(Hunger <= 0)
        {
            this.isAlive = false;
        }
    }

public void Hung(int hung)

    {
        Hunger += hung;
    }

public void Mony(int mony)
    {
        money += mony;
    }

public void Expi(int expi)
    {
        this.Exp += expi;
    }

public void LevelUp()
    {
    bool levelupprogres = true;
      while(levelupprogres)
      {

        if(Exp >= 100 * level)
            {
                
                Exp = Exp - 100*level;
                level++;
                UpdateLvlStats();
                System.Console.WriteLine(Exp);
                System.Console.WriteLine($"Новый уровень! {level}");
            }
        else{levelupprogres = false;}

      }
    }

protected void SearchFoodFight(Random dice, Player player, Enemy enemy)
    {
        System.Console.WriteLine($"Пока ты искал еду на тебя напал {enemy.name}!");
        enemy.GenerateLevel(player.level, dice);
        Combat.Fight(player, enemy);
        Hung(100);
    }
}