namespace NotRunescape;

public static class DropAnalytics
{
    public static void DisplayDropStatistics(List<BossLog> bossLogs)
    {
        Console.WriteLine("\n--- Boss Drop Statistics ---");
        int totalKills = bossLogs.Count;

        if (totalKills == 0)
        {
            Console.WriteLine("No kills logged yet! Unique drop rate: 0.00%");
            return;
        }

        int uniqueCount = bossLogs.Count(l => l.IsUnique);
        double percentage = ((double)uniqueCount / totalKills) * 100.0;

        Console.WriteLine($"Total Kills Logged : {totalKills}");
        Console.WriteLine($"Unique Drops       : {uniqueCount}");
        Console.WriteLine($"Unique Drop Rate   : {percentage:F2}%");
    }
}