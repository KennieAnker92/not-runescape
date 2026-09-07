using NotRunescape;

var bossLogs = new List<BossLog>();
var player = new Player();
var highScores = new HighScores();
Shop store = new();

Console.WriteLine("=== OSRS Boss & Combat Tracker ===");
Console.WriteLine("What is your character name? ");

var characterName = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(characterName))
    characterName = "Adventurer";

Console.WriteLine($"Welcome to Gielinor, {characterName}");

player.SetStartingGold(100);

while (true)
{
    Console.WriteLine($"\n[HP: {player.CurrentHp}/{player.MaxHp} | Gold: {player.Gold} GP]");
    Console.Write("[1] Log Boss Kill  [2] View Drop Log  [3] View Inventory  [4] Drop Item  [5] Rest at Lumbridge  [6] View High Scores [7] Visit Store [8] Drop Statistics [99] Fight Hill Giant  [0] Exit\nChoice: ");
    var input = Console.ReadLine()?.Trim();

    if (input == "0") break;

    if (input == "1")
    {
        Console.Write("Boss Name (e.g., Zulrah, Vorkath): ");
        string boss = Console.ReadLine() ?? "Unknown";

        Console.Write("Valuable Drop (e.g., Tanzanite Fang, None): ");
        string drop = Console.ReadLine() ?? "None";

        Console.Write("Did you get a unique drop? (y/n): ");
        bool isUnique = Console.ReadLine()?.Trim().ToLower() == "y";

        bossLogs.Add(new BossLog { BossName = boss, DropName = drop, IsUnique = isUnique });
        Console.WriteLine("Kill logged!");
    }
    else if (input == "2")
    {
        Console.WriteLine("\n--- Drop Log ---");
        if (bossLogs.Count == 0) Console.WriteLine("No drops logged yet!");
        else Console.WriteLine("You have " + bossLogs.Count + " drops logged.");
        
        for (int i = 0; i < bossLogs.Count; i++)
        {
            var log = bossLogs[i];
            string status = log.IsUnique ? "UNIQUE DROP!" : "Normal Drop";
            Console.WriteLine($"#{i + 1}: {log.BossName} - Drop: {log.DropName} [{status}] ({log.Timestamp:HH:mm})");
        }
    }
    else if (input == "3")
    {
        player.PrintInventory();
    }
    else if (input == "4")
    {
        HandleDropItem(player);
    }
    else if (input == "5")
    {
        player.ResetHealth();
    }
    else if (input == "6")
    {
        highScores.DisplayTopHits();
    }
    else if (input == "99")
    {
        StartGiantFight(player, bossLogs, highScores);
    }
    else if (input == "7")
    {
        store.OpenStore(player);
    }
    else if (input == "8")
    {
        DropAnalytics.DisplayDropStatistics(bossLogs);
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
    string quantityInput = Console.ReadLine()?.Trim() ?? "";

    // Exercise 10 Requirement: Guard against invalid parsing and negative/zero quantities
    if (!int.TryParse(quantityInput, out int amount) || amount <= 0)
    {
        Console.WriteLine("Invalid quantity. Please enter a positive whole number.");
        return;
    }

    if (player.DropItem(itemToDrop, amount))
    {
        Console.WriteLine($"Successfully dropped {amount}x {itemToDrop}.");
    }
    else
    {
        Console.WriteLine($"You do not have {amount}x {itemToDrop} to drop.");
    }
}

static void StartGiantFight(Player player, List<BossLog> bossLogs, HighScores highScores)
{
    if (player.CurrentHp <= 0)
    {
        Console.WriteLine("\nYou are too weak to fight! Respawning at Lumbridge...");
        player.CurrentHp = player.MaxHp;
        return;
    }

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("=== HILL GIANT CAVE ===");
    Console.WriteLine("A wild Hill Giant (Level 28) blocks your path!\n");
    Console.ResetColor();

    int giantHp = 35;
    var rng = new Random();

    while (player.CurrentHp > 0 && giantHp > 0)
    {
        Console.WriteLine($"Your HP: {player.CurrentHp}/{player.MaxHp} | Hill Giant HP: {giantHp}");
        Console.Write("Action: [1] Slash with Rune Scimitar  [2] Eat Lobster  [3] Special Attack (50 GP)  [4] Flee Choice: ");
        var choice = Console.ReadLine()?.Trim();

        if (choice == "1")
        {
            int playerHit = rng.Next(0, 15);
            giantHp -= playerHit;
            highScores.RecordHit(playerHit);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nYou slash the Hill Giant for a {playerHit}!");
            Console.ResetColor();
        }
        else if (choice == "2")
        {
            if (player.Inventory.ContainsKey("Lobster") && player.Inventory["Lobster"] > 0)
            {
                player.Inventory["Lobster"]--;
                player.CurrentHp = Math.Min(player.MaxHp, player.CurrentHp + 12);
                Console.WriteLine($"\nYou ate a Lobster! Restored HP to {player.CurrentHp}.");
            }
            else
            {
                Console.WriteLine("\nYou don't have any Lobsters in your inventory!");
            }
        }
        else if (choice == "3")
        {
            if (player.Gold < 50)
            {
                Console.WriteLine("\nYou don't have enough GP to use a special attack! (Requires 50 GP)");
            }
            else
            {
                player.Gold -= 50;
                int hit1 = rng.Next(0, 10);
                int hit2 = rng.Next(0, 10);
                
                highScores.RecordHit(hit1);
                highScores.RecordHit(hit2);
                
                int totalHit = hit1 + hit2;
                giantHp -= totalHit;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYou unleash a Special Attack! Hits: {hit1} and {hit2} (Total {totalHit})");
                Console.ResetColor();
            }
        }
        else if (choice == "4")
        {
            Console.WriteLine("\nYou flee from the Hill Giant! Returning to Lumbridge...");
            player.CurrentHp = player.MaxHp;
            return;
        }
        else
        {
            Console.WriteLine("\nInvalid choice! Please select a valid action.");
        }            

        if (giantHp > 0)
        {
            int giantHit = rng.Next(0, 6);
            player.CurrentHp -= giantHit;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"The Hill Giant swings his club for {giantHit} damage!\n");
            Console.ResetColor();
        }
    }

    if (player.CurrentHp > 0)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nVICTORY! The Hill Giant collapses!");
        Console.ResetColor();

        // Selective Loot Prompt
        var droppedItems = new List<(string Name, bool IsUnique)>
        {
            ("Big Bones", false),
            ("Limpwurt Root", false),
            ("Giant Key", true)
        };

        Console.WriteLine("\n--- Ground Loot ---");
        foreach (var drop in droppedItems)
        {
            Console.Write($"Pick up {drop.Name}? (y/n): ");
            var choice = Console.ReadLine()?.Trim().ToLower();

            if (choice == "y")
            {
                player.AddItem(drop.Name, 1);
                bossLogs.Add(new BossLog
                {
                    BossName = "Hill Giant",
                    DropName = drop.Name,
                    IsUnique = drop.IsUnique
                });
                Console.WriteLine($"Picked up 1x {drop.Name} and logged it!");
            }
            else
            {
                Console.WriteLine($"Left {drop.Name} on the ground.");
            }
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOh dear, you are dead! Teleporting back to Lumbridge...");
        player.CurrentHp = player.MaxHp;
        Console.ResetColor();
    }
}