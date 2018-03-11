using RestSharp;

namespace BasketApiClient
{
    public class BasketClientFactory
    {
        public IBasketClient Create(BasketClientSettings settings)
        {
            return new BasketClient(settings, new RestClient(), new RestRequestBuilder());
        }
    }
}
