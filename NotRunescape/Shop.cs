namespace NotRunescape;

public class Shop
{
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

            if (choice == "1") BuyItem(player, "Lobster", 20);
            else if (choice == "2") BuyItem(player, "Strength Potion", 50);
            else Console.WriteLine("Invalid choice!");
        }
    }

    private static void BuyItem(Player player, string item, int price)
    {
        if (player.Gold < price)
        {
            Console.WriteLine($"You don't have enough gold! (Requires {price} GP)");
            return;
        }

        player.Gold -= price;
        player.AddItem(item, 1);
        Console.WriteLine($"Purchased 1x {item} for {price} GP!");
    }
}