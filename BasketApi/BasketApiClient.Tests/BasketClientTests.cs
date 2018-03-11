using Moq;
using RestSharp;
using System;
using Xunit;

namespace BasketApiClient.Tests
{
    public class BasketClientTests
    {
        [Fact]
        public void CreateBasket_CreatesBasket()
        {
            var expectedBasket = new Basket() { Id = Guid.NewGuid() };
            var dummyRequest = new RestRequest() { Method = Method.POST, Resource = "baskets" };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(
                x => x.Execute<Basket>(dummyRequest))
                .Returns(new RestResponse<Basket> { Data = expectedBasket });
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(dummyRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.CreateBasket();

            Assert.Equal(expectedBasket.Id, sut.BasketId);
        }

        [Fact]
        public void CreateBasket_CallsApi()
        {
            var expectedBasket = new Basket();
            var expectedRequest = new RestRequest() { Method = Method.POST, Resource = "baskets" };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(
                x => x.Execute<Basket>(
                    expectedRequest))
                .Returns(new RestResponse<Basket> { Data = expectedBasket });
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(expectedRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.CreateBasket();

            restClientMock.VerifyAll();
        }

        [Fact]
        public void GetBasket_ReturnsBasket()
        {
            var expectedBasket = new Basket() {  Id = Guid.NewGuid() };
            var dummyRequest = new RestRequest() { Method = Method.GET, Resource = "baskets" };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(
                x => x.Execute<Basket>(dummyRequest))
                    .Returns(new RestResponse<Basket> { Data = expectedBasket });
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(dummyRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            var actual = sut.GetBasket();

            Assert.Equal(expectedBasket.Id, actual.Id);
        }

        [Fact]
        public void AddItem_AddsItemToBasket()
        {
            var expectedItem = new Item { code = "Arduino", quantity = 42 };
            var expectedRequest = new RestRequest() { Method = Method.PUT };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(expectedRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.AddItem(expectedItem);

            restRequestFactoryMock.Verify(x => x.AddBody(expectedItem));
            restClientMock.Verify(x => x.Execute(expectedRequest));
        }

        [Fact]
        public void UpdateItem_UpdatesItemInBasket()
        {
            var expectedItem = new Item { code = "Arduino", quantity = 42 };
            var expectedRequest = new RestRequest() { Method = Method.PUT };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(expectedRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.UpdateItem(expectedItem);

            restRequestFactoryMock.Verify(x => x.AddBody(expectedItem));
            restClientMock.Verify(x => x.Execute(expectedRequest));
        }

        [Fact]
        public void RemoveItem_RemoveItemFromBasket()
        {
            var expectedItemCode = "Arduino";
            var expectedRequest = new RestRequest() { Method = Method.DELETE };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(expectedRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.RemoveItem(expectedItemCode);

            restClientMock.Verify(x => x.Execute(expectedRequest));
        }

        [Fact]
        public void Clearbasket_ClearsBasket()
        {
            var expectedRequest = new RestRequest() { Method = Method.POST };
            var dummySettings = new BasketClientSettings() { BaseUrl = "https://api.co.uk" };
            var restClientMock = new Mock<IRestClient>();
            var restRequestFactoryMock = new Mock<IRestRequestBuilder>();
            restRequestFactoryMock.Setup(x => x.Build()).Returns(expectedRequest);
            var sut = new BasketClient(dummySettings, restClientMock.Object, restRequestFactoryMock.Object);

            sut.Clear();

            restClientMock.Verify(x => x.Execute(expectedRequest));
        }
    }
}
