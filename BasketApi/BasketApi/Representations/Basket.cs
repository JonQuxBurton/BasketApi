using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketApi.Representations
{
    public class Basket
    {
        public Guid Id { get; private set; }
        public List<Item> Items { get; private set; }

        public Basket(Guid id)
        {
            this.Id = id;
            this.Items = new List<Item>();
        }
    }
}
