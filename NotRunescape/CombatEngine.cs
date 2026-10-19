namespace NotRunescape;

/// <summary>
/// Exercise 16: Generic Combat Engine capable of fighting any Monster instance.
/// Handles prayer buffs, special attacks, weapon bonuses, and flee mechanics.
/// </summary>
public class CombatEngine
{
    private readonly Random _rng = new();

    public void FightMonster(Player player, Monster monster, List<BossLog> bossLogs, HighScores highScores)
    {
        if (player.CurrentHp <= 0)
        {
            Console.WriteLine("\nYou are too weak to fight! Respawning at Lumbridge...");
            player.ResetHealth();
            return;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"=== {monster.Name.ToUpper()} LAIR ===");
        Console.WriteLine($"A wild {monster.Name} (Level {monster.Level}) blocks your path!\n");
        Console.ResetColor();

        // Exercise 15: Prayer turn counter
        int prayerTurnsRemaining = 0;

        while (player.CurrentHp > 0 && monster.CurrentHp > 0)
        {
            int maxHitBonus = player.EquippedWeapon?.MaxHitBonus ?? 0;
            int playerMaxHit = 10 + maxHitBonus;

            // Exercise 13: ASCII Energy Bar Rendering
            string asciiBar = RenderAsciiBar(player.SpecialEnergy, Player.MaxSpecialEnergy);

            Console.WriteLine($"Your HP: {player.CurrentHp}/{player.MaxHp} | Spec Energy: [{asciiBar}] {player.SpecialEnergy}% | {monster.Name} HP: {monster.CurrentHp}/{monster.MaxHp}");
            Console.Write("Action: [1] Attack  [2] Eat Lobster  [3] Special Attack  [4] Protect Prayer (10 GP)  [5] Run Away\nChoice: ");
            var choice = Console.ReadLine()?.Trim();

            bool validTurnTaken = true;

            if (choice == "1")
            {
                int playerHit = _rng.Next(0, playerMaxHit + 1);
                monster.CurrentHp -= playerHit;

                // Exercise 9: Record hit to High Scores
                highScores.RecordHit(playerHit);

                // Exercise 13: Recharge 10% special energy per normal hit
                player.RechargeSpecialEnergy(10);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYou attack {monster.Name} for {playerHit} damage!");
                Console.ResetColor();
            }
            else if (choice == "2")
            {
                // Exercise 5: Eat Lobster overheal check
                if (player.EatLobster())
                {
                    Console.WriteLine($"\nYou ate a Lobster! Restored HP to {player.CurrentHp}/{player.MaxHp}.");
                }
                else
                {
                    Console.WriteLine("\nYou don't have any Lobsters in your inventory!");
                    validTurnTaken = false;
                }
            }
            else if (choice == "3")
            {
                // Exercise 6 & 13: Special Attack draining 50% energy and rolling two hit checks
                if (player.SpecialEnergy < 50)
                {
                    Console.WriteLine("\nYou don't have enough Special Energy! Requires 50%.");
                    validTurnTaken = false;
                }
                else
                {
                    player.ConsumeSpecialEnergy(50);
                    int hit1 = _rng.Next(0, playerMaxHit);
                    int hit2 = _rng.Next(0, playerMaxHit);
                    int totalHit = hit1 + hit2;

                    monster.CurrentHp -= totalHit;

                    // Exercise 9: Record special attack hits
                    highScores.RecordHit(hit1);
                    highScores.RecordHit(hit2);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nYou unleash a Special Attack! Hits: {hit1} and {hit2} (Total {totalHit})");
                    Console.ResetColor();
                }
            }
            else if (choice == "4")
            {
                // Exercise 15: Activate Protection Prayer for 10 GP
                if (player.Gold < 10)
                {
                    Console.WriteLine("\nYou need at least 10 GP to activate Protection Prayer!");
                    validTurnTaken = false;
                }
                else
                {
                    player.Gold -= 10;
                    prayerTurnsRemaining = 3;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nActivated Protect from Melee! Incoming damage halved for 3 turns.");
                    Console.ResetColor();
                }
            }
            else if (choice == "5")
            {
                // Exercise 8: 50% Run Away chance
                bool escaped = _rng.Next(0, 2) == 0;
                if (escaped)
                {
                    Console.WriteLine($"\nYou successfully fled from the {monster.Name}!");
                    return; // Break fight loop cleanly
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nYou failed to flee from the {monster.Name}!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid choice!");
                validTurnTaken = false;
            }

            // Monster Retaliation Phase
            if (monster.CurrentHp > 0 && validTurnTaken)
            {
                int rawMonsterHit = _rng.Next(0, monster.MaxHit + 1);
                int finalHit = rawMonsterHit;

                // Exercise 15: Protect from Melee damage reduction and turn decrementing
                if (prayerTurnsRemaining > 0)
                {
                    finalHit /= 2;
                    prayerTurnsRemaining--;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Protect Active: Halved damage ({rawMonsterHit} -> {finalHit}) | {prayerTurnsRemaining} turns remaining]");
                    Console.ResetColor();
                }

                player.CurrentHp -= finalHit;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The {monster.Name} hits you for {finalHit} damage!\n");
                Console.ResetColor();
            }
        }

        // Post-combat resolution
        if (player.CurrentHp > 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nVICTORY! You defeated the {monster.Name}!");
            Console.ResetColor();

            foreach (var drop in monster.DropTable)
            {
                Console.Write($"Pick up {drop.Name}? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    player.AddItem(drop.Name, 1);
                    bossLogs.Add(new BossLog
                    {
                        BossName = monster.Name,
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
            player.ResetHealth();
            Console.ResetColor();
        }
    }

    private static string RenderAsciiBar(int current, int max)
    {
        int totalBlocks = 10;
        int filled = (int)Math.Round((double)current / max * totalBlocks);
        return new string('=', filled) + new string('-', totalBlocks - filled);
    }
}