namespace NotRunescape;

public static class SpriteRenderer
{
    // ==========================================
    // EASY EXERCISES (1–2)
    // ==========================================

    // Exercise 1: Green Hitsplat Color Helper (1–5 lines)
    public static void DrawDamageHitsplat(int damage, bool isPlayer)
    {
        Console.BackgroundColor = isPlayer ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" -{damage} HP ");
        Console.ResetColor();
        Console.WriteLine();
    }

    // Exercise 2: Gold Coin Stack Graphic (1–5 lines)
    public static void DrawGold(int amount)
    {
        int icons = Math.Max(1, amount / 50);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Gold [{amount} GP]: ");
        for (int i = 0; i < icons; i++) Console.Write("[$] ");
        Console.ResetColor();
        Console.WriteLine();
    }

    // ==========================================
    // MEDIUM EXERCISES (3–4)
    // ==========================================

    // Exercise 3: Visual Segmented Health Bar (5–10 lines)
    public static void DrawHealthBar(int current, int max)
    {
        int totalBlocks = 10;
        int filled = (int)Math.Round((double)current / max * totalBlocks);
        filled = Math.Clamp(filled, 0, totalBlocks);

        Console.Write("HP: [");
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.Write(new string(' ', filled));
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.Write(new string(' ', totalBlocks - filled));
        Console.ResetColor();
        Console.WriteLine($"] {current}/{max}");
    }

    // Exercise 4: Color-Coded Item Rarity Renderer (5–10 lines)
    public static void PrintLootItem(string itemName, bool isUnique)
    {
        if (isUnique)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"★ UNIQUE DROP: {itemName} ★");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Normal Drop: {itemName}");
        }
        Console.ResetColor();
    }

    // ==========================================
    // CHALLENGING EXERCISES (5–6)
    // ==========================================

    // Exercise 5: Animated Boss Death Banner (11–15 lines)
    public static void DrawVictoryBanner(string bossName)
    {
        string text = $"*** VICTORY OVER {bossName.ToUpper()} ***";
        string border = new string('=', text.Length);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n{border}");
        Thread.Sleep(100);
        Console.WriteLine(text);
        Thread.Sleep(100);
        Console.WriteLine($"{border}\n");
        Console.ResetColor();
    }

    // Exercise 6: Dynamic Inventory Grid UI (11–15 lines)
    public static void DrawInventoryBox(Dictionary<string, int> inventory)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("+------------------------------+");
        Console.WriteLine("|       PLAYER INVENTORY       |");
        Console.WriteLine("+------------------------------+");
        
        foreach (var item in inventory)
        {
            string line = $"| {item.Key} x{item.Value}";
            Console.WriteLine(line.PadRight(31) + "|");
        }
        
        Console.WriteLine("+------------------------------+");
        Console.ResetColor();
    }

    // ==========================================
    // HARD EXERCISES (7–8)
    // ==========================================

    // Exercise 7: Special Energy Gauge Renderer (16–30 lines)
    public static void DrawSpecialEnergyBar(int currentEnergy)
    {
        currentEnergy = Math.Clamp(currentEnergy, 0, 100);
        int totalBlocks = 10;
        int filled = currentEnergy / 10;

        ConsoleColor barColor = currentEnergy switch
        {
            100 => ConsoleColor.Cyan,
            >= 50 => ConsoleColor.Yellow,
            _ => ConsoleColor.Red
        };

        Console.Write("SPEC: [");
        Console.BackgroundColor = barColor;
        Console.Write(new string(' ', filled));
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.Write(new string(' ', totalBlocks - filled));
        Console.ResetColor();
        Console.WriteLine($"] {currentEnergy}%");
    }

    // Exercise 8: Combat Log Screen Overlay (16–30 lines)
    public static void RenderCombatScreen(Player player, string monsterName, int monsterHp, int monsterMaxHp)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"==================== COMBAT: {monsterName.ToUpper()} ====================");
        Console.ResetColor();

        Console.Write("Player ");
        DrawHealthBar(player.CurrentHp, player.MaxHp);

        Console.Write($"{monsterName} ");
        DrawHealthBar(monsterHp, monsterMaxHp);

        Console.WriteLine(@"
          (o_o) 
         <|   |>  [Hill Giant]
          /   \  
        ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Actions: [1] Slash  [2] Eat Lobster  [3] Special Attack  [4] Run");
        Console.WriteLine("------------------------------------------------------------");
        Console.ResetColor();
    }
}