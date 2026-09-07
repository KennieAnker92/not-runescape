namespace NotRunescape;

public class SpriteRenderer
{
    public static void DrawDamageHitsplat(int damage, bool isPlayer)
    {
        Console.BackgroundColor = isPlayer ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" -{damage} HP ");
        Console.ResetColor();
        Console.WriteLine();
    }
    public static void DrawGold(int amount)
    {
        int icons = Math.Max(1, amount / 50);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Gold [{amount} GP]: ");
        for (int i = 0; i < icons; i++) Console.Write("[$] ");
        Console.ResetColor();
        Console.WriteLine();
    }
}