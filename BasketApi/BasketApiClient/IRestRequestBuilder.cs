using RestSharp;

namespace BasketApiClient
{
    public interface IRestRequestBuilder
    {
        void Setup(Method method, string resource);
        void AddBody(object body);
        IRestRequest Build();
    }
}
