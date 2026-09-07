namespace NotRunescape;

public class Player
{
    // Exercise 2: StarterGold public const elimination of magic numbers
    public const int StarterGold = 100;

    public int CurrentHp { get; set; } = 35;
    public int MaxHp { get; set; } = 35;
    public int Gold { get; set; } = StarterGold;
    
    // Exercise 13: Special Energy Bar System (0 - 100%)
    public int SpecialEnergy { get; set; } = 100;
    public const int MaxSpecialEnergy = 100;

    // Exercise 14: Equipped Weapon Tracking
    public Weapon? EquippedWeapon { get; set; } = new Weapon("Rune Scimitar", 5);

    // Exercise 7: Case-insensitive inventory using StringComparer.OrdinalIgnoreCase
    public Dictionary<string, int> Inventory { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Lobster", 3 },
        { "Abyssal Whip", 1 }
    };

    public void SetStartingGold(int gold)
    {
        Gold = gold;
    }

    public void AddItem(string item, int amount)
    {
        if (Inventory.ContainsKey(item))
            Inventory[item] += amount;
        else
            Inventory[item] = amount;
    }

    public bool DropItem(string item, int amount)
    {
        if (!Inventory.ContainsKey(item) || Inventory[item] < amount)
        {
            return false;
        }

        // Exercise 10: Deduct amount and remove key entirely if 0
        Inventory[item] -= amount;
        if (Inventory[item] <= 0)
        {
            Inventory.Remove(item);
        }
        return true;
    }

    // Exercise 5: Overheal cap protection when consuming food
    public bool EatLobster()
    {
        if (Inventory.ContainsKey("Lobster") && Inventory["Lobster"] > 0)
        {
            Inventory["Lobster"]--;
            CurrentHp = Math.Min(MaxHp, CurrentHp + 12);
            if (Inventory["Lobster"] <= 0) Inventory.Remove("Lobster");
            return true;
        }
        return false;
    }

    // Exercise 13: Special energy consumption and recharging caps
    public void ConsumeSpecialEnergy(int amount)
    {
        SpecialEnergy = Math.Max(0, SpecialEnergy - amount);
    }

    public void RechargeSpecialEnergy(int amount)
    {
        SpecialEnergy = Math.Min(MaxSpecialEnergy, SpecialEnergy + amount);
    }

    // Exercise 14: Swapping equipped weapon and returning previous to inventory
    public void EquipWeapon(Weapon newWeapon)
    {
        if (EquippedWeapon != null)
        {
            AddItem(EquippedWeapon.Name, 1);
        }

        DropItem(newWeapon.Name, 1);
        EquippedWeapon = newWeapon;
        Console.WriteLine($"\nEquipped {newWeapon.Name}! (+{newWeapon.MaxHitBonus} Max Hit Bonus)");
    }

    // Exercise 3: Health and energy reset
    public void ResetHealth()
    {
        CurrentHp = MaxHp;
        SpecialEnergy = MaxSpecialEnergy;
    }

    public void PrintInventory()
    {
        Console.WriteLine("\n--- Inventory ---");
        if (Inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        foreach (var item in Inventory)
        {
            Console.WriteLine($"- {item.Key}: {item.Value}");
        }
    }
}