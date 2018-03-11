using BasketApi.Controllers;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using BasketApi.Domain;
using Moq;
using System;
using BasketApi.Representations;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Tests.Controllers
{
    public class ItemsControllerTests
    {
        private ItemsController sut;
        private Mock<IBasketsRepository> basketsRepositoryMock;

        public ItemsControllerTests()
        {
            basketsRepositoryMock = new Mock<IBasketsRepository>();
            sut = new ItemsController(basketsRepositoryMock.Object);
        }

        [Fact]
        public void Put_AddsNewItem()
        {
            var expectedItem = new Item { code = "Arduino" };
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            sut.Put(expectedBasket.Id, expectedItem);

            basketsRepositoryMock.Verify(x => x.AddItemToBasket(expectedBasket.Id, expectedItem));
        }

        [Fact]
        public void Put_ReturnsNoContent()
        {
            var expectedItem = new Item { code = "Arduino" };
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            var actual = sut.Put(expectedBasket.Id, expectedItem);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void Put_UpdatesAnExistingItem()
        {
            var expectedBasket = new Basket(Guid.NewGuid());
            var expectedItem = new Item { code = "Arduino", quantity = 42};
            var existingItem = new Item { code = "Arduino", quantity = 1 };
            basketsRepositoryMock.Setup(x => x.GetItem(expectedBasket.Id, existingItem.code))
                .Returns(existingItem);
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);

            sut.Put(expectedBasket.Id, expectedItem);

            basketsRepositoryMock.Verify(x => x.UpdateItemInBasket(expectedBasket.Id, expectedItem));
        }

        [Fact]
        public void Put_WhenItemIsNull_ReturnsBadRequest()
        {
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Put(dummyBasketId, null);

            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public void Put_WhenBasketNotFound_ReturnsNotFound()
        {
            var noneExistentBasketId = Guid.NewGuid();
            var dummyItem = new Item { code = "Arduino" };

            var actual = sut.Put(noneExistentBasketId, dummyItem);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void Delete_RemovesAnItem()
        {
            var expectedItemCode = "Arduino";
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);
            basketsRepositoryMock.Setup(x => x.GetItem(expectedBasket.Id, expectedItemCode))
                .Returns(new Item { code = expectedItemCode });

            sut.Delete(expectedBasket.Id, expectedItemCode);

            basketsRepositoryMock.Verify(x => x.RemoveItemFromBasket(expectedBasket.Id, expectedItemCode));
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            var dummyItemCode = "Arduino";
            var dummyBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(dummyBasket.Id))
                .Returns(dummyBasket);
            basketsRepositoryMock.Setup(x => x.GetItem(dummyBasket.Id, dummyItemCode))
                .Returns(new Item { code = dummyItemCode });

            var actual = sut.Delete(dummyBasket.Id, dummyItemCode);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void Delete_WhenBasketNotFound_ReturnsNotFound()
        {
            var nonExistentBasketId = Guid.NewGuid();
            var dummyItemCode = "Arduino";
            
            var actual = sut.Delete(nonExistentBasketId, dummyItemCode);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void Delete_WhenItemNotFound_ReturnsNotFound()
        {
            var dummyItemCode = "Arduino";
            var dummyBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(dummyBasket.Id))
                .Returns(dummyBasket);

            var actual = sut.Delete(dummyBasket.Id, dummyItemCode);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void Get_ReturnsAnItem()
        {
            var expectedItemCode = "Arduino";
            var expectedBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(expectedBasket.Id))
                .Returns(expectedBasket);
            basketsRepositoryMock.Setup(x => x.GetItem(expectedBasket.Id, expectedItemCode))
                .Returns(new Item { code = expectedItemCode });

            var actual = sut.Get(expectedBasket.Id, expectedItemCode);

            Assert.IsType<OkObjectResult>(actual);
            var actualResult = actual as OkObjectResult;
            Assert.IsType<Item>(actualResult.Value);
            Assert.Equal(expectedItemCode, (actualResult.Value as Item).code);
        }

        [Fact]
        public void Get_WhenBasketNotFound_ReturnsNotFound()
        {
            var dummyItemCode = "Arduino";
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Get(dummyBasketId, dummyItemCode);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void Get_WhenItemNotFound_ReturnsNotFound()
        {
            var dummyItemCode = "Arduino";
            var dummyBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(dummyBasket.Id))
                .Returns(dummyBasket);

            var actual = sut.Get(dummyBasket.Id, dummyItemCode);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void GetItemsForBasket_ReturnsAllItems()
        {
            var expectedItems = new List<Item>() {
                new Item{ code = "Arduino" },
                new Item{ code = "BBC micro:bit" }
            };
            var dummyBasket = new Basket(Guid.NewGuid());
            basketsRepositoryMock.Setup(x => x.GetBasket(dummyBasket.Id))
                .Returns(dummyBasket);
            basketsRepositoryMock.Setup(x => x.GetItemsForBasket(dummyBasket.Id))
                .Returns(expectedItems);

            var actual = sut.GetItemsForBasket(dummyBasket.Id);

            Assert.IsType<OkObjectResult>(actual);
            var actualResult = actual as OkObjectResult;
            var actualItems = (actualResult.Value as IEnumerable<Item>);
            Assert.True(actualItems.Contains(expectedItems.First()));
        }

        [Fact]
        public void GetItemsForBasket_WhenBasketNotFound_ReturnsNotFound()
        {
            var expectedBasketId = Guid.NewGuid();

            var actual = sut.GetItemsForBasket(expectedBasketId);

            Assert.IsType<NotFoundResult>(actual);
        }
    }
}
