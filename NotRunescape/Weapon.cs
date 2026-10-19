namespace NotRunescape;

public class Weapon
{
    public string Name { get; set; }
    public int MaxHitBonus { get; set; }

    public Weapon(string name, int maxHitBonus)
    {
        Name = name;
        MaxHitBonus = maxHitBonus;
    }
}