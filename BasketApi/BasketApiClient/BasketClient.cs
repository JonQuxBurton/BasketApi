using RestSharp;
using System;

namespace BasketApiClient
{
    public class BasketClient : IBasketClient
    {
        private readonly BasketClientSettings settings;
        private readonly IRestClient restClient;
        private readonly IRestRequestBuilder restRequestBuilder;

        public Guid BasketId { get; private set; }

        public BasketClient(BasketClientSettings settings, 
            IRestClient restClient,
            IRestRequestBuilder restRequestFactory)
        {
            this.settings = settings;
            this.restClient = restClient;
            this.restRequestBuilder = restRequestFactory;
        }

        /// <summary>
        /// A basic check for connectivity and if the API is available.
        /// </summary>
        /// <returns>True if available, else false.</returns>
        public bool CanConnect()
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.GET, $"status");
            var request = restRequestBuilder.Build();

            IRestResponse response = restClient.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Creates a new Basket.
        /// </summary>
        public void CreateBasket()
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.POST, $"baskets");
            var request = restRequestBuilder.Build();

            var response = restClient.Execute<Basket>(request);

            BasketId = response.Data.Id;
        }

        /// <summary>
        /// Gets the Basket previously created by CreatBasket..
        /// </summary>
        /// <returns>The Basket.</returns>
        public Basket GetBasket()
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.GET, $"baskets/{BasketId}");
            var request = restRequestBuilder.Build();

            var response = restClient.Execute<Basket>(request);

            return response.Data;
        }
        
        /// <summary>
        /// Adds an Item to the current Basket.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);
            
            restRequestBuilder.Setup(Method.PUT, $"baskets/{BasketId}/items");
            restRequestBuilder.AddBody(item);
            var request = restRequestBuilder.Build();
            request.AddHeader("Content-Type", "application/json");

            restClient.Execute(request);
        }

        /// <summary>
        /// Updates an Item in the current Basket.
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(Item item)
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.PUT, $"baskets/{BasketId}/items");
            restRequestBuilder.AddBody(item);
            var request = restRequestBuilder.Build();

            restClient.Execute(request);
        }

        /// <summary>
        /// Removes an Item from the current Basket.
        /// </summary>
        /// <param name="itemCode"></param>
        public void RemoveItem(string itemCode)
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.DELETE, $"baskets/{BasketId}/items/{itemCode}");
            var request = restRequestBuilder.Build();

            restClient.Execute(request);
        }

        /// <summary>
        /// Clears the current Basket.
        /// </summary>
        public void Clear()
        {
            restClient.BaseUrl = new Uri(settings.BaseUrl);

            restRequestBuilder.Setup(Method.POST, $"baskets/{BasketId}/clear");
            var request = restRequestBuilder.Build();
            
            restClient.Execute(request);
        }
    }
}
