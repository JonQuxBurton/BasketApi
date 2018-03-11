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
                new Item{ code = "Arduino" },
                new Item{ code = "BBC micro:bit" }
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
                .Returns(new Item { code = expectedItemCode });
            var actual = sut.Get(expectedBasketId, expectedItemCode);

            Assert.IsType<OkObjectResult>(actual);
            var actualResult = actual as OkObjectResult;
            Assert.IsType<Item>(actualResult.Value);
            Assert.Equal(expectedItemCode, (actualResult.Value as Item).code);
        }

        [Fact]
        public void PutAddsAnNewItem()
        {
            var expectedItem = new Item { code = "Arduino" };
            var expectedBasketId = Guid.NewGuid();
            
            sut.Put(expectedBasketId, expectedItem);

            basketsRepositoryMock.Verify(x => x.AddItemToBasket(expectedBasketId, expectedItem));
        }

        [Fact]
        public void PutReturnsNoContent()
        {
            var expectedItem = new Item { code = "Arduino" };
            var expectedBasketId = Guid.NewGuid();

            var actual = sut.Put(expectedBasketId, expectedItem);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void PutUpdatesAnExistingItem()
        {
            var expectedBasketId = Guid.NewGuid();
            var expectedItem = new Item { code = "Arduino", quantity = 42};
            var existingItem = new Item { code = "Arduino", quantity = 1 };
            basketsRepositoryMock.Setup(x => x.GetItem(expectedBasketId, existingItem.code))
                .Returns(existingItem);

            sut.Put(expectedBasketId, expectedItem);

            basketsRepositoryMock.Verify(x => x.UpdateItemInBasket(expectedBasketId, expectedItem));
        }

        [Fact]
        public void DeleteRemovesAnItem()
        {
            var expectedItemCode = "Arduino";
            var expectedBasketId = Guid.NewGuid();

            sut.Delete(expectedBasketId, expectedItemCode);

            basketsRepositoryMock.Verify(x => x.RemoveItemFromBasket(expectedBasketId, expectedItemCode));
        }

        [Fact]
        public void DeleteReturnsNoContent()
        {
            var dummyItemCode = "Arduino";
            var dummyBasketId = Guid.NewGuid();

            var actual = sut.Delete(dummyBasketId, dummyItemCode);

            Assert.IsType<NoContentResult>(actual);
        }
    }
}
