using BasketApi.Domain;
using BasketApi.Representations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BasketApi.Controllers
{
    [Route("api/baskets/{basketId}/items")]
    public class ItemsController : Controller
    {
        private readonly IBasketsRepository basketRepository;

        public ItemsController(IBasketsRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet]
        public IEnumerable<Item> GetItemsForBasket(Guid basketId)
        {
            return this.basketRepository.GetItemsForBasket(basketId);
        }

        [HttpGet("{itemCode}")]
        public IActionResult Get(Guid basketId, string itemCode)
        {
            var item = this.basketRepository.GetItem(basketId, itemCode);

            return new OkObjectResult(item);
        }

        [HttpPut]
        public IActionResult Put(Guid basketId, [FromBody] Item item)
        {
            var existingItem = this.basketRepository.GetItem(basketId, item.code);

            if (existingItem != null)
                this.basketRepository.UpdateItemInBasket(basketId, item);
            else
                this.basketRepository.AddItemToBasket(basketId, item);

            return NoContent();
        }

        [HttpDelete("{itemCode}")]
        public IActionResult Delete(Guid basketId, string itemCode)
        {
            this.basketRepository.RemoveItemFromBasket(basketId, itemCode);

            return NoContent();
        }
    }
}
