using BasketApi.Domain;
using System;
using System.Collections.Generic;
using BasketApi.Representations;
using System.Linq;

namespace BasketApi.DataAccess
{
    public class InMemoryBasketsRepository : IBasketsRepository
    {
        private List<Basket> baskets = new List<Basket>();
        
        public Basket GetBasket(Guid basketId)
        {
            return this.baskets.First(x => x.Id == basketId);
        }

        public void AddItemToBasket(Guid basketId, Item itemToAdd)
        {
            this.GetBasket(basketId).Items.Add(itemToAdd);
        }

        public void ClearBasket(Guid basketId)
        {
            this.GetBasket(basketId).Items.RemoveAll(x => true);
        }

        public Basket CreateBasket()
        {
            var basket = new Basket(Guid.NewGuid());
            this.baskets.Add(basket);

            return basket;
        }

        public Item GetItem(Guid basketId, string itemCode)
        {
            return this.GetBasket(basketId).Items.First(y => y.Code == itemCode);
        }

        public IEnumerable<Item> GetItemsForBasket(Guid basketId)
        {
            return this.GetBasket(basketId).Items;
        }

        public void RemoveItemFromBasket(Guid basketId, string itemCode)
        {
            this.GetBasket(basketId).Items.RemoveAll(x => x.Code == itemCode);
        }
    }
}
