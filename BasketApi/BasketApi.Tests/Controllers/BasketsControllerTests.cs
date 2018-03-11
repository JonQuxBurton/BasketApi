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
    public class BasketsControllerTests
    {
        private BasketsController sut;
        private Mock<IBasketsRepository> basketsRepositoryMock;

        public BasketsControllerTests()
        {
            this.basketsRepositoryMock = new Mock<IBasketsRepository>();
            this.sut = new BasketsController(basketsRepositoryMock.Object);
        }

        [Fact]
        public void Post_CreatesBasket()
        {
            basketsRepositoryMock.Setup(x => x.CreateBasket())
                .Returns(new Basket(Guid.NewGuid()));

            sut.Post();
            
            basketsRepositoryMock.Verify(x => x.CreateBasket());
        }
        
        [Fact]
        public void Post_ReturnsCreatedBasket()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.CreateBasket()).Returns(expectedBasket);

            var actual = sut.Post();

            var actualResult = actual as CreatedAtRouteResult;
            Assert.IsType<Basket>(actualResult.Value);
            Assert.Equal(expectedBasket.Id, (actualResult.Value as Basket).Id);
        }

        [Fact]
        public void Post_ReturnsCreated()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.CreateBasket()).Returns(expectedBasket);

            var actual = sut.Post();

            Assert.IsType<CreatedAtRouteResult>(actual);
        }

        [Fact]
        public void Clear_ClearsBasket()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            sut.Clear(expectedBasket.Id);

            basketsRepositoryMock.Verify(x => x.ClearBasket(expectedBasket.Id));
        }

        [Fact]
        public void Clear_ReturnsNoContent()
        {
            var dummyBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(dummyBasket.Id))
                .Returns(dummyBasket);
            var actual = sut.Clear(dummyBasket.Id);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void Clear_WhenBasketNotFound_ReturnsNotFound()
        {
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Clear(dummyBasketId);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void Get_ReturnsBasket()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            expectedBasket.Items.Add(new Item { code = "Arduino" });
            expectedBasket.Items.Add(new Item { code = "BBC micro:bit" });
            expectedBasket.Items.Add(new Item { code = "Raspberry Pi" });
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            var actual = sut.Get(expectedBasket.Id);

            var actualResult = actual as OkObjectResult;
            Assert.Equal(200, actualResult.StatusCode);
            Assert.IsType<Basket>(actualResult.Value);
            var actualBasket = actualResult.Value as Basket;
            Assert.Equal(expectedBasket.Id, actualBasket.Id);
            Assert.Equal(expectedBasket.Items.Count, actualBasket.Items.Count);
            Assert.NotNull(actualBasket.Items.First(x => x.code == expectedBasket.Items.First().code));
            Assert.NotNull(actualBasket.Items.First(x => x.code == expectedBasket.Items.ElementAt(1).code));
            Assert.NotNull(actualBasket.Items.First(x => x.code == expectedBasket.Items.ElementAt(2).code));
        }

        [Fact]
        public void Get_WhenBasketNotFound_ReturnsNotFound()
        {
            var dummyExpectedId = Guid.NewGuid();

            var actual = sut.Get(dummyExpectedId);

            Assert.IsType<NotFoundResult>(actual);
        }
    }
}