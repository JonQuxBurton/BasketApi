namespace BasketApiClient
{
    public interface IBasketClient
    {
        bool CanConnect();
        Basket GetBasket();
        void CreateBasket();
        void AddItem(Item item);
        void RemoveItem(string itemCode);
        void Clear();
        void UpdateItem(Item item);
    }
}
