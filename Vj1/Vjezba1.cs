using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;



Character[] party = { new Warrior("Captain IV", 50, 12), new Healer("Medic II", 30, 5, 3),
                      new Dueler("Shortsword IX", 45, 10, 6)};

Enemy[] field = { new Skeleton("Small Bone", 20, 7), new Skeleton("Calficator", 40, 12),
                  new Dragon("Bahamut", 90, 12, 4, 3)  };

Inventory<IItem> inventory = new Inventory<IItem>();
inventory.AddItem(new Potion("Small Health Potion", 10));
inventory.AddItem(new Potion("Large Health Potion", 30));
inventory.AddItem(new Bomb("Grenade", 30));
inventory.AddItem(new Bomb("Firecracker", 10));

listParty();
listField();
listItems();

PartyAttackInOrder();
listField();
EnemiesAttackInOrder();
listParty();
PartyAttackInOrder();
listField();

Dragon drag1 = (Dragon)field[2];
drag1.AttackSweep(party);
listParty();

listItems();
Potion selectedPotion = (Potion)inventory.Items[0];
selectedPotion.Use(party[0]);
inventory.RemoveItem(selectedPotion);
listItems();
Healer healer1 = (Healer)party[1];
healer1.HealAlly(party[0]);
Dueler duel1 = (Dueler)party[2];
duel1.Parry();
listParty();

field[1].Attack(party[1]);
field[2].Attack(party[2]);
listParty();
listField();

Warrior warrior1 = (Warrior)party[0];
warrior1.ChargeAttack(field[1]);
Bomb selectedBomb = (Bomb)inventory.Items[1];
selectedBomb.Throw(field[2]);
inventory.RemoveItem(selectedBomb);
listItems();
party[2].Attack(field[2]);
listParty();
listField();

EnemiesAttackInOrder();
listParty();
PartyAttackInOrder();
listField();
drag1.AttackSweep(party);
listParty();
PartyAttackInOrder();
listField();
EnemiesAttackInOrder();
listParty();
listField();



void PartyAttackInOrder()
{
    foreach (Character c in party)
    {
        if (c.Health > 0)
        {
            int i = 0;
            foreach (Enemy e in field)
            {
                while (field[i].Health <= 0) { i++; }
            }
            if (field[i] != null)
                c.Attack(field[i]);
        }
    }
}
void EnemiesAttackInOrder()
{
    foreach (Enemy e in field)
    {
        if (e.Health > 0)
        {
            int i = 0;
            foreach (Character c in party)
            {
                while (party[i].Health <= 0) { i++; }
            }
            if (party[i] != null)
                e.Attack(party[i]);
        }
    }
}
void listItems()
{
    Debug.Print("\nItems:");
    int i = 0;
    foreach (var item in inventory.Items)
    {
        i++;
        Debug.Print($"{i}. {item.Name}");
    }
    Debug.Print("");
}
void listParty()
{
    Debug.Print("\nParty:");
    int i = 0;
    foreach (Character c in party)
    {
        i++;
        Debug.Print($"{i}. Name: {c.Name}, HP: {c.Health}/{c.MaxHealth}, Strength: {c.Strength}");
    }
    Debug.Print("");
}
void listField()
{
    Debug.Print("\nEnemies:");
    int i = 0;
    foreach (Enemy e in field)
    {
        i++;
        Debug.Print($"{i}. Name: {e.Name}, HP: {e.Health}/{e.MaxHealth}, Strength: {e.Strength}");
    }
    Debug.Print("");
}

public interface IAttack
{
    void Attack(IDamageable target);
}


public interface IDamageable
{
    string Name { get; }
    void TakeDamage(int damage);
}

public interface IHealable
{
    void Heal(int hpAmount);
}

public interface IItem
{
    string Name { get; }
}
public interface IUsable : IItem
{
    void Use(Character target);
}
public interface IThrowable : IItem
{
    void Throw(Enemy target);
}


public abstract class Character : IDamageable, IAttack, IHealable
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Strength { get; private set; }

    public Character(string name, int health, int strength)
    {
        Name = name;
        Health = (int)MathF.Max(0, health);
        MaxHealth = Health;
        Strength = strength;
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0) { return; }

        int comparison = Health;
        Health = (int)MathF.Max(0, Health - damage);
        Debug.Print($"{Name} took {comparison-Health} damage!");
        if (Health == 0)
            Debug.Print($"{Name} is defeated!");
    }
    public virtual void Heal(int hpAmount)
    {
        if (hpAmount <= 0) {return; }
        Health = (int)MathF.Min(MaxHealth, Health += hpAmount);
        Debug.Print($"{Name} healed {hpAmount}hp.");
    }

    public abstract void Attack(IDamageable target);
}

public abstract class Enemy : IDamageable, IAttack
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Strength { get; private set; }

    public Enemy(string name, int health, int strength)
    {
        Name = name;
        Health = (int)MathF.Max(0, health);
        MaxHealth = Health;
        Strength = strength;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) { return; }

        int comparison = Health;
        Health = (int)MathF.Max(0, Health - damage);
        Debug.Print($"{Name} took {comparison - Health} damage!");
        if (Health == 0)
            Debug.Print($"{Name} is defeated!");
    }

    public abstract void Attack(IDamageable target);
}


public class Skeleton : Enemy
{
    public Skeleton(string name, int health, int strength) : base(name, health, strength) { }
    public override void Attack(IDamageable target) {
        Debug.Print($"{Name} attacked {target.Name}.");
        target.TakeDamage(Strength);
    }
}
public class Dragon : Enemy
{
    public int DamageStep { get; private set; }
    public int StepTargets { get; private set; }
    public Dragon(string name, int health, int strength, int dmgStep, int stepTargets) : base(name, health, strength) 
    {
        DamageStep = dmgStep;
        StepTargets = stepTargets;
    }
    public override void Attack(IDamageable target)
    {
        Debug.Print($"{Name} attacked {target.Name}.");
        target.TakeDamage(Strength);
    }
    public void AttackSweep(Character[] party)
    {
        int damage = Strength - DamageStep;
        int maxStep = Strength + (DamageStep * StepTargets) + 1;
        Debug.Print($"{Name}'s fire breath burns the whole party.");
        foreach (Character targetc in party)
        {
            damage = (int)MathF.Max(Strength, (damage + DamageStep) % maxStep);
            targetc.TakeDamage(damage);
        }
    }
}

public class Warrior : Character
{
    public Warrior(string name, int health, int strength) : base(name, health, strength) { }
    public void ChargeAttack(IDamageable target)
    {
        Debug.Print($"{Name} charged in and dealt {(int)(Strength * 1.3)} damage to {target}!");
        target.TakeDamage((int)(Strength * 1.3));
        Debug.Print($"{Name} was hurt by recoil.");
        TakeDamage(MaxHealth / 10);
    }
    public override void Attack(IDamageable target)
    {
        Debug.Print($"{Name} attacked {target.Name}");
        target.TakeDamage(Strength);
    }
}
public class Dueler : Character
{
    public int ParryDamage { get; private set; }
    public bool Parrying { get; private set; }
    public bool Parried { get; private set; }

    public Dueler(string name, int health, int strength, int parryDmg) : base(name, health, strength) 
    {
        ParryDamage = parryDmg;
    }
    public void Parry()
    {
        Debug.Print($"{Name} prepares to parry.");
        Parrying = true;
    }
    public override void TakeDamage(int damage)
    {
        if (damage < 0) { return; }
        if (Parrying) {
            Parrying = false;
            Parried = true;
            Debug.Print($"{Name} parried the attack. Their next attack is stronger");
            damage = damage / 2;
        }
        base.TakeDamage(damage);
    }
    public override void Heal(int hpAmount)
    {
        if (Parrying) { Parrying = false; }
        if (hpAmount <= 0) { return; }
        base.Heal(hpAmount);
    }
    public override void Attack(IDamageable target)
    {
        if (Parrying) { Parrying = false; }
        Debug.Print($"{Name} attacked {target.Name}");
        if (Parried) {
            Parried = false;
            int parryAttack = Strength + ParryDamage;
            Debug.Print("Critical hit!");
            target.TakeDamage(parryAttack);
        }
        else
            target.TakeDamage(Strength);
    }
}
public class Healer : Character
{
    public int HealStrength { get; private set; }
    public Healer(string name, int health, int strength, int healMultiplier) : base(name, health, strength) 
    {
        HealStrength = MaxHealth / healMultiplier;
    }
    public void HealAlly(Character target)
    {
        Debug.Print($"{Name} casts heal on {target.Name}.");
        target.Heal(HealStrength);
    }
    public override void Attack(IDamageable target) 
    {
        Debug.Print($"{Name} attacked {target.Name}");
        target.TakeDamage(Strength);
    }
}


public class Inventory<T>
{
    private List<T> items = new List<T>();

    public void AddItem(T item) => items.Add(item);
    public void RemoveItem(T item) => items.Remove(item);

    public List<T> Items => items;
}

public class Potion : IUsable
{
    public string Name { get; private set; }
    public int HealAmount { get; private set; }
    public Potion(string name, int hpAmount)
    {
        Name = name;
        HealAmount = hpAmount;
    }
    public void Use(Character target)
    {
        Debug.Print($"{Name} used on {target.Name}.");
        target.Heal(HealAmount);
    }
}

public class Bomb : IThrowable
{
    public string Name { get; private set; }
    public int DamageAmount { get; private set; }
    public Bomb(string name, int dmgAmount)
    {
        Name = name;
        DamageAmount = dmgAmount;
    }
    public void Throw(Enemy target)
    {
        Debug.Print($"{Name} thrown at {target.Name}.");
        target.TakeDamage(DamageAmount);
    }
}
