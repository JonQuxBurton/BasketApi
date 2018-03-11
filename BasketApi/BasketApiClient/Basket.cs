using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketApiClient
{
    public class Basket
    {
        public Guid Id { get; set; }
        public List<Item> Items { get; set; }
        
        public override string ToString()
        {
            var items = "{empty}";

            if (Items.Count == 0)
                items = string.Join("\n", Items.Select(x => $"{x.code}, {x.quantity}"));

            return $"Basket: {Id}\n" + items;
        }
    }
}
