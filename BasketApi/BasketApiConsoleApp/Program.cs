using BasketApiClient;
using System;

namespace BasketApiConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new BasketClientSettings
            {
                // BaseUrl = "http://localhost:57332/api/"
                BaseUrl = "http://localhost:57331/api/"

            };

            var client = new BasketClientFactory().Create(settings);

            Console.WriteLine($"\nBegin test");

            var canConnect = client.CanConnect();
            Console.WriteLine($"Can connect to API: {canConnect}");

            Console.WriteLine($"\nCreating basket...");
            client.CreateBasket();
            var basket = client.GetBasket();
            Console.WriteLine(basket.ToString());

            Console.WriteLine($"\nAdding items...");
            client.AddItem(new Item { code = "Arduino", quantity = 42 });
            client.AddItem(new Item { code = "BBC micro:bit", quantity = 101 });
            client.AddItem(new Item { code = "RaspberryPi", quantity = 202 });
            basket = client.GetBasket();
            Console.WriteLine(basket.ToString());

            Console.WriteLine($"\nUpdating Arduino quantity to 4...");
            client.UpdateItem(new Item { code = "Arduino", quantity = 4 });
            basket = client.GetBasket();
            Console.WriteLine(basket.ToString());

            Console.WriteLine($"\nRemoving RaspberryPi...");
            client.RemoveItem("RaspberryPi");
            basket = client.GetBasket();
            Console.WriteLine(basket.ToString());

            Console.WriteLine($"\nClearing basket...");
            client.Clear();
            basket = client.GetBasket();
            Console.WriteLine(basket.ToString());

            Console.WriteLine($"\nTest completed");

            Console.WriteLine("\nPress any key to close");
            Console.ReadKey();
        }
    }
}
