namespace NotRunescape;

public class WorldMap
{
    // ==========================================
    // EASY ISSUES (#101–#102)
    // ==========================================

    // Issue #102: Set Dungeon Spawn Coordinates (1–5 lines)
    public int PlayerX { get; set; } = 1;
    public int PlayerY { get; set; } = 1;

    // Issue #201: Dungeon Grid Map Data (5–10 lines)
    private readonly char[,] grid = new char[8, 8]
    {
        { '#', '#', '#', '#', '#', '#', '#', '#' },
        { '#', '.', '.', '.', '#', '.', '.', '#' },
        { '#', '.', '#', '.', '#', '.', 'G', '#' },
        { '#', '.', '#', '.', '.', '.', '.', '#' },
        { '#', '.', '#', '#', '#', '#', '.', '#' },
        { '#', '.', '.', '.', '.', '#', '.', '#' },
        { '#', '.', 'G', '.', '.', '.', '.', '#' },
        { '#', '#', '#', '#', '#', '#', '#', '#' }
    };

    // Issue #101: Map Legend Footer (1–5 lines)
    public void PrintMapLegend()
    {
        // Changed from DarkGray to White for high contrast readability
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n[Legend: @ = You | G = Hill Giant | # = Wall | . = Path]");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("[Controls: W/A/S/D = Move | Q = Exit Map]");
        Console.ResetColor();
    }

    public void DrawMap()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("==================== DUNGEON MAP ====================\n");
        Console.ResetColor();

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (x == PlayerX && y == PlayerY)
                {
                    // Bright Cyan for player `@` makes it instantly pop
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("@ ");
                    Console.ResetColor();
                }
                else
                {
                    char tile = grid[y, x];

                    if (tile == '#')
                    {
                        // Changed from DarkGray to White/Gray for clear boundaries
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (tile == 'G')
                    {
                        // Bright Red with DarkRed background highlight
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        // Faint dots for floor tiles so playable elements stand out
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write(tile + " ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }

        PrintMapLegend();
    }
    // ==========================================
    // CHALLENGING ISSUES (#301–#302)
    // ==========================================

    // Issue #301 & #302: Map Boundary & Wall Collision Engine (11–15 lines)
    public bool CanMoveTo(int newX, int newY)
    {
        // Issue #301: Boundary bounds checking
        if (newX < 0 || newX >= grid.GetLength(1) || newY < 0 || newY >= grid.GetLength(0))
        {
            return false;
        }

        // Issue #302: Wall collision checking
        if (grid[newY, newX] == '#')
        {
            return false;
        }

        return true;
    }

    // Helper to clear a defeated monster tile from the grid
    public void ClearTile(int x, int y)
    {
        grid[y, x] = '.';
    }

    // ==========================================
    // HARD ISSUES (#401–#402)
    // ==========================================

    // Issue #401 & #402: 2D WASD Movement Loop & Encounter Triggers (16–30 lines)
    public void ExploreDungeon(Player player, List<BossLog> bossLogs, Action<Player, List<BossLog>> startCombatCallback)
    {
        bool exploring = true;

        while (exploring)
        {
            DrawMap();

            // Check if standing on a Monster tile 'G'
            if (grid[PlayerY, PlayerX] == 'G')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[!] AMBUSH! A Hill Giant attacks you!");
                Console.ResetColor();
                Console.WriteLine("Press ENTER to fight...");
                Console.ReadLine();

                // Trigger combat system
                startCombatCallback(player, bossLogs);

                // Clear monster from map if player survived
                if (player.CurrentHp > 0)
                {
                    ClearTile(PlayerX, PlayerY);
                }
                else
                {
                    // Respawn player at start on death
                    PlayerX = 1;
                    PlayerY = 1;
                }
                continue;
            }

            // Read key press without echoing character to console
            ConsoleKey key = Console.ReadKey(true).Key;

            int targetX = PlayerX;
            int targetY = PlayerY;

            switch (key)
            {
                case ConsoleKey.W: targetY--; break;
                case ConsoleKey.S: targetY++; break;
                case ConsoleKey.A: targetX--; break;
                case ConsoleKey.D: targetX++; break;
                case ConsoleKey.Q: exploring = false; break;
            }

            // Apply movement if collision passes
            if (CanMoveTo(targetX, targetY))
            {
                PlayerX = targetX;
                PlayerY = targetY;
            }
        }
    }
}