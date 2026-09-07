namespace NotRunescape;

public class Monster
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public int MaxHit { get; set; }
    public List<(string Name, bool IsUnique)> DropTable { get; set; }

    public Monster(string name, int level, int maxHp, int maxHit, List<(string, bool)> dropTable)
    {
        Name = name;
        Level = level;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        MaxHit = maxHit;
        DropTable = dropTable;
    }
}