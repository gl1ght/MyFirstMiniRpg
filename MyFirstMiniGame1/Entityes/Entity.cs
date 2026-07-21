
abstract class Entity
{
    Random dice = new Random();
    public string name{get; protected set;}
    protected int health;
    protected int damage;
    public int MaxHealth{ get; protected set; }
    public bool isAlive = true;
    public int level{get; protected set;}
    public int basedamage{get; protected set;}
    public int baseHealth{get; protected set;}
    public string description{get; protected set;}


    public int Damage{get{return damage;}  set
            {
                damage = value;
                
            }}
            
    public int Health{get{return health;} protected set
            {
            if (value > MaxHealth)
                health = MaxHealth;
            else if (value < 0)
                health = 0;
            else
                health = value;
            }}


public Entity(int level, int baseHealthValue, int baseDamage, string name, string description)
{
    this.level = level;
    this.baseHealth = baseHealthValue;
    this.basedamage = baseDamage;
    this.name = name;
    this.description = description;

    UpdateLvlStats();
   
    Health = MaxHealth;
}
public void AliveCheckByHP()
    {
        if(Health <= 0)
        {
            this.isAlive = false;
        }
    }

public void Heal(int heal)
    {
        Health += heal;
    }
public void Heal(int heal, bool percentHeal)
{
    if (percentHeal)
        Health += MaxHealth * heal / 100;
    else
        Health += heal;
}

public void TakeDamage(int damage)
{
    Health -= damage;


    if (Health == 0)
        isAlive = false;
}

public void UpdateLvlStats()
{
    MaxHealth = baseHealth + (level - 1) * 15;
    Health = MaxHealth;
    Damage = basedamage + (level - 1) * 3;
}

public void AddDamage(int amount)
{
    Damage += amount;
}

}