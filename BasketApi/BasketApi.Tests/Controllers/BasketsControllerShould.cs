using BasketApi.Controllers;
using BasketApi.Domain;
using BasketApi.Representations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace BasketApi.Tests.Controllers
{
    public class BasketsControllerShould
    {
        private BasketsController sut;
        private Mock<IBasketsRepository> basketsRepositoryMock;

        public BasketsControllerShould()
        {
            this.basketsRepositoryMock = new Mock<IBasketsRepository>();
            this.sut = new BasketsController(basketsRepositoryMock.Object);
        }

        [Fact]
        public void PostCreatesBasket()
        {
            sut.Post();
            
            basketsRepositoryMock.Verify(x => x.CreateBasket());
        }
        
        [Fact]
        public void PostReturnsCreatedBasket()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.CreateBasket()).Returns(expectedBasket);

            var actual = sut.Post();

            var actualResult = actual as OkObjectResult;
            Assert.Equal(200, actualResult.StatusCode);
            Assert.IsType<Basket>(actualResult.Value);
            Assert.Equal(expectedBasket.Id, (actualResult.Value as Basket).Id);
        }

        [Fact]
        public void ClearClearsBasket()
        {
            var expectedBasketId = Guid.NewGuid();

            sut.Clear(expectedBasketId);

            basketsRepositoryMock.Verify(x => x.ClearBasket(expectedBasketId));
        }

        [Fact]
        public void ClearReturnsNoContent()
        {
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Clear(dummyBasketId);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void GetReturnsBasket()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            expectedBasket.Items.Add(new Item { Code = "Arduino" });
            expectedBasket.Items.Add(new Item { Code = "BBC micro:bit" });
            expectedBasket.Items.Add(new Item { Code = "Raspberry Pi" });
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            var actual = sut.Get(expectedBasket.Id);

            var actualResult = actual as OkObjectResult;
            Assert.Equal(200, actualResult.StatusCode);
            Assert.IsType<Basket>(actualResult.Value);
            var actualBasket = actualResult.Value as Basket;
            Assert.Equal(expectedBasket.Id, actualBasket.Id);
            Assert.Equal(expectedBasket.Items.Count, actualBasket.Items.Count);
            Assert.NotNull(actualBasket.Items.First(x => x.Code == expectedBasket.Items.First().Code));
            Assert.NotNull(actualBasket.Items.First(x => x.Code == expectedBasket.Items.ElementAt(1).Code));
            Assert.NotNull(actualBasket.Items.First(x => x.Code == expectedBasket.Items.ElementAt(2).Code));
        }
    }
}