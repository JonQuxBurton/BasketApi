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
    public class ItemsControllerShould
    {
        private ItemsController sut;
        private Mock<IBasketsRepository> basketsRepositoryMock;

        public ItemsControllerShould()
        {
            basketsRepositoryMock = new Mock<IBasketsRepository>();
            sut = new ItemsController(basketsRepositoryMock.Object);
        }

        [Fact]
        public void GetReturnsAllItems()
        {
            var expectedItems = new List<Item>() {
                new Item{ Code = "Arduino" },
                new Item{ Code = "BBC micro:bit" }
            };
            var dummyBasketId = Guid.NewGuid();
            basketsRepositoryMock.Setup(x => x.GetItemsForBasket(dummyBasketId))
                .Returns(expectedItems.AsEnumerable());

            var actual = sut.GetItemsForBasket(dummyBasketId);

            Assert.True(actual.Contains(expectedItems.First()));
        }

        [Fact]
        public void GetReturnsAnItem()
        {
            var expectedItemCode = "Arduino";
            var expectedBasketId = Guid.NewGuid();
            basketsRepositoryMock.Setup(x => x.GetItem(expectedBasketId, expectedItemCode))
                .Returns(new Item { Code = expectedItemCode });
            var actual = sut.Get(expectedBasketId, expectedItemCode);

            Assert.IsType<OkObjectResult>(actual);
            var actualResult = actual as OkObjectResult;
            Assert.IsType<Item>(actualResult.Value);
            Assert.Equal(expectedItemCode, (actualResult.Value as Item).Code);
        }

        [Fact]
        public void PutAddsAnItem()
        {
            var expectedItem = new Item { Code = "APP01" };
            var expectedBasketId = Guid.NewGuid();
            
            sut.Put(expectedBasketId, expectedItem);

            basketsRepositoryMock.Verify(x => x.AddItemToBasket(expectedBasketId, expectedItem));
        }

        [Fact]
        public void PutReturnsNoContent()
        {
            var expectedItem = new Item { Code = "APP01" };
            var expectedBasketId = Guid.NewGuid();

            var actual = sut.Put(expectedBasketId, expectedItem);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void DeleteRemovesAnItem()
        {
            var expectedItemCode = "APP01";
            var expectedBasketId = Guid.NewGuid();

            sut.Delete(expectedBasketId, expectedItemCode);

            basketsRepositoryMock.Verify(x => x.RemoveItemFromBasket(expectedBasketId, expectedItemCode));
        }

        [Fact]
        public void DeleteReturnsNoContent()
        {
            var dummyItemCode = "APP01";
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Delete(dummyBasketId, dummyItemCode);

            Assert.IsType<NoContentResult>(actual);
        }
    }
}
