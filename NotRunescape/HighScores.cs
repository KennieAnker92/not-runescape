namespace NotRunescape;

/// <summary>
/// Tracks player combat hits and provides leaderboard analytics.
/// </summary>
public class HighScores
{
    private readonly List<int> _hitHistory = new();

    /// <summary>
    /// Records a damage value dealt to a monster.
    /// </summary>
    public void RecordHit(int damage)
    {
        _hitHistory.Add(damage);
    }

    /// <summary>
    /// Displays the top 3 highest hits dealt across all combat sessions.
    /// Handles empty lists without throwing exceptions.
    /// </summary>
    public void DisplayTopHits()
    {
        Console.WriteLine("\n--- Combat High Scores (Top Hits) ---");

        // Edge case handling: Guard against empty lists
        if (_hitHistory.Count == 0)
        {
            Console.WriteLine("No combat hits recorded yet! Go fight some monsters.");
            return;
        }

        // Use LINQ to order by damage descending and take top 3
        var topHits = _hitHistory
            .OrderByDescending(hit => hit)
            .Take(3)
            .ToList();

        for (int i = 0; i < topHits.Count; i++)
        {
            Console.WriteLine($"#{i + 1}: {topHits[i]} Damage");
        }
    }
}