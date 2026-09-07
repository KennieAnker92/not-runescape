using NotRunescape;

var bossLogs = new List<BossLog>();
var player = new Player();
var highScores = new HighScores();
var store = new Shop();
var combatEngine = new CombatEngine();

Console.WriteLine("=== OSRS Boss & Combat Tracker ===");
Console.Write("What is your character name?: ");
var characterName = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(characterName))
    characterName = "Adventurer";

Console.WriteLine($"Welcome to Gielinor, {characterName}!");

player.SetStartingGold(Player.StarterGold);

bool running = true;

while (running)
{
    string equippedName = player.EquippedWeapon?.Name ?? "None";
    int equippedBonus = player.EquippedWeapon?.MaxHitBonus ?? 0;

    Console.WriteLine($"\n[HP: {player.CurrentHp}/{player.MaxHp} | Gold: {player.Gold} GP | Spec: {player.SpecialEnergy}% | Weapon: {equippedName} (+{equippedBonus})]");
    Console.WriteLine("[1] Log Boss Kill          [2] View Drop Log           [3] View Inventory");
    Console.WriteLine("[4] Drop Item              [5] Rest at Lumbridge       [6] View High Scores");
    Console.WriteLine("[7] General Store          [8] Drop Statistics         [9] Equip Weapon");
    Console.WriteLine("[99] Fight Monster         [0] Exit");
    Console.Write("Choice: ");

    var input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            LogKill(bossLogs);
            break;

        case "2":
            PrintDropLog(bossLogs);
            break;

        case "3":
            player.PrintInventory();
            break;

        case "4":
            HandleDropItem(player);
            break;

        case "5":
            player.ResetHealth();
            Console.WriteLine("\nYou rest at Lumbridge. Your HP and Special Energy have been fully restored!");
            break;

        case "6":
            highScores.DisplayTopHits();
            break;

        case "7":
            store.OpenStore(player);
            break;

        case "8":
            DropAnalytics.DisplayDropStatistics(bossLogs);
            break;

        case "9":
            HandleEquipWeapon(player);
            break;

        case "99":
            SelectAndFightMonster(player, bossLogs, highScores, combatEngine);
            break;

        case "0":
            running = false;
            Console.WriteLine("Goodbye!");
            break;

        default:
            Console.WriteLine("Invalid option! Please enter a number from the menu.");
            break;
    }
}

static void LogKill(List<BossLog> bossLogs)
{
    Console.Write("\nBoss Name (e.g., Zulrah, Vorkath): ");
    string boss = Console.ReadLine() ?? "Unknown";

    Console.Write("Valuable Drop (e.g., Tanzanite Fang, None): ");
    string drop = Console.ReadLine() ?? "None";

    Console.Write("Did you get a unique drop? (y/n): ");
    bool isUnique = Console.ReadLine()?.Trim().ToLower() == "y";

    bossLogs.Add(new BossLog { BossName = boss, DropName = drop, IsUnique = isUnique });
    Console.WriteLine("Kill logged!");
}

static void PrintDropLog(List<BossLog> bossLogs)
{
    Console.WriteLine("\n--- Drop Log ---");
    if (bossLogs.Count == 0)
    {
        Console.WriteLine("No drops logged yet!");
        return;
    }

    Console.WriteLine($"You have {bossLogs.Count} total drops logged.\n");

    for (int i = 0; i < bossLogs.Count; i++)
    {
        var log = bossLogs[i];
        string status = log.IsUnique ? "UNIQUE DROP!" : "Normal Drop";
        Console.WriteLine($"#{i + 1}: {log.BossName} - Drop: {log.DropName} [{status}] ({log.Timestamp:HH:mm})");
    }
}

static void HandleDropItem(Player player)
{
    player.PrintInventory();
    if (player.Inventory.Count == 0) return;

    Console.Write("\nEnter the exact name of the item to drop: ");
    string itemToDrop = Console.ReadLine()?.Trim() ?? "";

    if (!player.Inventory.ContainsKey(itemToDrop))
    {
        Console.WriteLine("Item not found in inventory.");
        return;
    }

    Console.Write($"How many '{itemToDrop}' would you like to drop?: ");
    
    if (int.TryParse(Console.ReadLine()?.Trim(), out int amount) && amount > 0)
    {
        if (player.DropItem(itemToDrop, amount))
        {
            Console.WriteLine($"Successfully dropped {amount}x {itemToDrop}.");
        }
        else
        {
            Console.WriteLine("You don't have enough of that item to drop.");
        }
    }
    else
    {
        Console.WriteLine("Invalid amount. Must be a positive integer.");
    }
}

static void HandleEquipWeapon(Player player)
{
    Console.WriteLine("\n--- Equipment Management ---");
    Console.WriteLine($"Currently Equipped: {(player.EquippedWeapon != null ? player.EquippedWeapon.Name + " (+" + player.EquippedWeapon.MaxHitBonus + " Bonus)" : "None")}");
    
    var knownWeapons = new List<Weapon>
    {
        new Weapon("Rune Scimitar", 5),
        new Weapon("Abyssal Whip", 9),
        new Weapon("Bronze Spear", 2)
    };

    var availableWeapons = knownWeapons.Where(w => player.Inventory.ContainsKey(w.Name) && player.Inventory[w.Name] > 0).ToList();

    if (availableWeapons.Count == 0)
    {
        Console.WriteLine("No equipable weapons found in inventory!");
        return;
    }

    Console.WriteLine("\nWeapons available in inventory:");
    for (int i = 0; i < availableWeapons.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {availableWeapons[i].Name} (+{availableWeapons[i].MaxHitBonus} Max Hit)");
    }
    Console.Write("Select a weapon to equip (0 to cancel): ");

    if (int.TryParse(Console.ReadLine()?.Trim(), out int choice) && choice > 0 && choice <= availableWeapons.Count)
    {
        player.EquipWeapon(availableWeapons[choice - 1]);
    }
}

static void SelectAndFightMonster(Player player, List<BossLog> bossLogs, HighScores highScores, CombatEngine engine)
{
    Console.WriteLine("\n=== Select Monster ===");
    Console.WriteLine("[1] Goblin (Level 2)");
    Console.WriteLine("[2] Hill Giant (Level 28)");
    Console.WriteLine("[3] Moss Giant (Level 42)");
    Console.Write("Choice: ");

    var choice = Console.ReadLine()?.Trim();

    Monster monster = choice switch
    {
        "1" => new Monster("Goblin", 2, 12, 3, new List<(string, bool)> { ("Bronze Spear", true) }),
        "3" => new Monster("Moss Giant", 42, 60, 10, new List<(string, bool)> { ("Big Bones", false), ("Ranarr Seed", true) }),
        _ => new Monster("Hill Giant", 28, 35, 6, new List<(string, bool)> { ("Big Bones", false), ("Limpwurt Root", false), ("Giant Key", true) })
    };

    engine.FightMonster(player, monster, bossLogs, highScores);
}