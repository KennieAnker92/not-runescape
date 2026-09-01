using NotRunescape;

var bossLogs = new List<BossLog>();
var player = new Player();

Console.WriteLine("=== OSRS Boss & Combat Tracker ===");
Console.WriteLine("What is your character name? ");

var characterName = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(characterName))
    characterName = "Adventurer";

Console.WriteLine($"Welcome to Gielinor, {characterName}");

player.SetStartingGold(100);

while (true)
{
    // Integrated EXERCISE 3 (Health Bar) & EXERCISE 2 (Gold Coins) on the main menu
    Console.WriteLine();
    SpriteRenderer.DrawHealthBar(player.CurrentHp, player.MaxHp);
    SpriteRenderer.DrawGold(player.Gold);

    Console.Write("[1] Log Boss Kill  [2] View Drop Log  [3] View Inventory  [4] Drop Item  [5] Rest  [6] Explore Map (WASD)  [99] Fight Hill Giant  [0] Exit\nChoice: ");
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
            
            // Integrated EXERCISE 4: Color-Coded Drop Rarity Renderer
            Console.Write($"#{i + 1}: {log.BossName} - ");
            SpriteRenderer.PrintLootItem(log.DropName, log.IsUnique);
        }
    }
    else if (input == "3")
    {
        // Integrated EXERCISE 6: Visual Inventory Box UI
        SpriteRenderer.DrawInventoryBox(player.Inventory);
    }
    else if (input == "4")
    {
        HandleDropItem(player);
    }
    else if (input == "5")
    {
        player.ResetHealth();
        Console.WriteLine("Resting at Lumbridge... HP Restored!");
    }
    else if (input == "6")
    {
        var map = new WorldMap();
        // Launches WASD movement and connects to combat on 'G' tiles
        map.ExploreDungeon(player, bossLogs, StartGiantFight); 
    }
    else if (input == "99")
    {
        StartGiantFight(player, bossLogs);
    }
}

static void HandleDropItem(Player player)
{
    SpriteRenderer.DrawInventoryBox(player.Inventory);
    if (player.Inventory.Count == 0) return;

    Console.Write("\nEnter the exact name of the item to drop: ");
    string itemToDrop = Console.ReadLine()?.Trim() ?? "";

    Console.Write("How many to drop?: ");
    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
    {
        if (player.DropItem(itemToDrop, amount))
        {
            Console.WriteLine($"Dropped {amount}x {itemToDrop}.");
        }
        else
        {
            Console.WriteLine("You don't have enough of that item to drop.");
        }
    }
    else
    {
        Console.WriteLine("Invalid amount.");
    }
}

static void StartGiantFight(Player player, List<BossLog> bossLogs)
{
    if (player.CurrentHp <= 0)
    {
        Console.WriteLine("\nYou are too weak to fight! Respawning at Lumbridge...");
        player.CurrentHp = player.MaxHp;
        return;
    }

    int giantHp = 35;
    int giantMaxHp = 35;
    int specialEnergy = 100; // Used for Exercise 7
    var rng = new Random();

    while (player.CurrentHp > 0 && giantHp > 0)
    {
        // Integrated EXERCISE 8: Render Full Combat Screen
        SpriteRenderer.RenderCombatScreen(player, "Hill Giant", giantHp, giantMaxHp);

        // Integrated EXERCISE 7: Render Special Energy Bar
        SpriteRenderer.DrawSpecialEnergyBar(specialEnergy);

        Console.Write("\nAction Choice: ");
        var choice = Console.ReadLine()?.Trim();

        if (choice == "1")
        {
            int playerHit = rng.Next(0, 15);
            giantHp -= playerHit;

            // Integrated EXERCISE 1: Green Player Hitsplat
            Console.Write("\nYou hit the Hill Giant:");
            SpriteRenderer.DrawDamageHitsplat(playerHit, isPlayer: true);
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
            // Integrated Special Attack using EXERCISE 7 & EXERCISE 1
            if (specialEnergy >= 50)
            {
                specialEnergy -= 50;
                int specHit1 = rng.Next(0, 12);
                int specHit2 = rng.Next(0, 12);
                giantHp -= (specHit1 + specHit2);

                Console.Write("\nSPECIAL ATTACK Hit 1:");
                SpriteRenderer.DrawDamageHitsplat(specHit1, isPlayer: true);
                Console.Write("SPECIAL ATTACK Hit 2:");
                SpriteRenderer.DrawDamageHitsplat(specHit2, isPlayer: true);
            }
            else
            {
                Console.WriteLine("\nNot enough Special Energy!");
            }
        }

        if (giantHp > 0)
        {
            int giantHit = rng.Next(0, 6);
            player.CurrentHp -= giantHit;

            // Integrated EXERCISE 1: Red Monster Hitsplat
            Console.Write("The Hill Giant hits you:");
            SpriteRenderer.DrawDamageHitsplat(giantHit, isPlayer: false);

            // Recharge special energy slightly per turn
            specialEnergy = Math.Min(100, specialEnergy + 10);
        }

        Console.WriteLine("\nPress ENTER to continue turn...");
        Console.ReadLine();
    }

    if (player.CurrentHp > 0)
    {
        // Integrated EXERCISE 5: Visual Victory Banner
        SpriteRenderer.DrawVictoryBanner("Hill Giant");

        // Selective Loot Prompt with EXERCISE 4 Color-Coded Rarity
        var droppedItems = new List<(string Name, bool IsUnique)>
        {
            ("Big Bones", false),
            ("Limpwurt Root", false),
            ("Giant Key", true)
        };

        Console.WriteLine("--- Ground Loot ---");
        foreach (var drop in droppedItems)
        {
            SpriteRenderer.PrintLootItem(drop.Name, drop.IsUnique);
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