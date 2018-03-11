using RestSharp;

namespace BasketApiClient
{
    public class BasketClientFactory
    {
        /// <summary>
        /// Creates a BasketClient from the provided settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>The newly created BasketClient.</returns>
        public IBasketClient Create(BasketClientSettings settings)
        {
            return new BasketClient(settings, new RestClient(), new RestRequestBuilder());
        }
    }
}
