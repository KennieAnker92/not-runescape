namespace NotRunescape;

/// <summary>
/// Handles purchasing items using player gold.
/// </summary>
public class Shop
{
    /// <summary>
    /// Opens the store sub-menu and handles item transactions.
    /// </summary>
    public void OpenStore(Player player)
    {
        while (true)
        {
            Console.WriteLine("\n=== EDGEVILLE GENERAL STORE ===");
            Console.WriteLine($"Your Gold: {player.Gold} GP");
            Console.WriteLine("[1] Buy Lobster (20 GP)");
            Console.WriteLine("[2] Buy Strength Potion (50 GP)");
            Console.WriteLine("[0] Leave Store");
            Console.Write("Choice: ");

            var choice = Console.ReadLine()?.Trim();

            if (choice == "0") break;

            if (choice == "1")
            {
                BuyItem(player, "Lobster", 20);
            }
            else if (choice == "2")
            {
                BuyItem(player, "Strength Potion", 50);
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }
    }

    private void BuyItem(Player player, string itemName, int price)
    {
        // Edge Case: Validate balance before deducting gold or adding item
        if (player.Gold < price)
        {
            Console.WriteLine($"You don't have enough gold! Need {price} GP, but you only have {player.Gold} GP.");
            return;
        }

        player.Gold -= price;
        player.AddItem(itemName, 1);
        Console.WriteLine($"Purchased 1x {itemName} for {price} GP!");
    }
}