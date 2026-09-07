namespace NotRunescape;

/// <summary>
/// Provides statistical analytics over boss drop logs.
/// </summary>
public static class DropAnalytics
{
    /// <summary>
    /// Calculates and prints unique drop rate metrics.
    /// Safely handles empty logs (division by zero).
    /// </summary>
    public static void DisplayDropStatistics(List<BossLog> bossLogs)
    {
        Console.WriteLine("\n--- Boss Drop Statistics ---");

        int totalKills = bossLogs.Count;

        // Edge Case: Division by zero prevention
        if (totalKills == 0)
        {
            Console.WriteLine("No kills logged yet! Unique drop rate is 0.00%.");
            return;
        }

        int uniqueCount = bossLogs.Count(log => log.IsUnique);
        double percentage = ((double)uniqueCount / totalKills) * 100.0;

        Console.WriteLine($"Total Logged Kills : {totalKills}");
        Console.WriteLine($"Unique Drops       : {uniqueCount}");
        Console.WriteLine($"Unique Drop Rate   : {percentage:F2}%");
    }
}