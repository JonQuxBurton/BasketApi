using BasketApi.Representations;
using System;
using System.Collections.Generic;

namespace BasketApi.Domain
{
    public interface IBasketsRepository
    {
        Basket CreateBasket();
        void AddItemToBasket(Guid basketId, Item itemToAdd);
        IEnumerable<Item> GetItemsForBasket(Guid basketId);
        void RemoveItemFromBasket(Guid basketId, string itemCode);
        void ClearBasket(Guid basketId);
        Item GetItem(Guid basketId, string itemCode);
        Basket GetBasket(Guid basketId);
        void UpdateItemInBasket(Guid basketId, Item item);
    }
}
