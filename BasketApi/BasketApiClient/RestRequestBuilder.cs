using RestSharp;

namespace BasketApiClient
{
    public class RestRequestBuilder : IRestRequestBuilder
    {
        private object body;
        private Method method;
        private string resource;

        public void AddBody(object body)
        {
            this.body = body;
        }

        public IRestRequest Build()
        {
            var request = new RestRequest(this.resource, this.method);
            
            if (this.body != null)
                request.AddJsonBody(this.body);

            return request;
        }

        public void Setup(Method method, string resource)
        {
            this.method = method;
            this.resource = resource;
            this.body = null;
        }
    }
}
