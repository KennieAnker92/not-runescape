namespace NotRunescape;

public class HighScores
{
    private readonly List<int> _hitHistory = new();

    public void RecordHit(int damage)
    {
        _hitHistory.Add(damage);
    }

    public void DisplayTopHits()
    {
        Console.WriteLine("\n--- Combat High Scores (Top Hits) ---");
        if (_hitHistory.Count == 0)
        {
            Console.WriteLine("No combat hits recorded yet!");
            return;
        }

        var topHits = _hitHistory.OrderByDescending(h => h).Take(3).ToList();
        for (int i = 0; i < topHits.Count; i++)
        {
            Console.WriteLine($"#{i + 1}: {topHits[i]} Damage");
        }
    }
}