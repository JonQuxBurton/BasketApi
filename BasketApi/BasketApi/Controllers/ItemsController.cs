using BasketApi.Domain;
using BasketApi.Representations;
using Microsoft.AspNetCore.Mvc;
using System;

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

        /// <summary>
        /// Gets an Item from a Basket.
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="itemCode"></param>
        /// <returns>200 OK and the Item</returns>
        [HttpGet("{itemCode}")]
        public IActionResult Get(Guid basketId, string itemCode)
        {
            var basket = this.basketRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();
            
            var item = this.basketRepository.GetItem(basketId, itemCode);

            if (item == null)
                return NotFound();

            return new OkObjectResult(item);
        }

        /// <summary>
        /// Adds an Item and Quantity to a Basket. If the Item is already in the Basket, the Quantity will be updated.
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="item"></param>
        /// <returns>204 No Content</returns>
        [HttpPut]
        public IActionResult Put(Guid basketId, [FromBody] Item item)
        {
            if (item == null)
                return BadRequest();

            var basket = this.basketRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            var existingItem = this.basketRepository.GetItem(basketId, item.code);

            if (existingItem != null)
                this.basketRepository.UpdateItemInBasket(basketId, item);
            else
                this.basketRepository.AddItemToBasket(basketId, item);

            return NoContent();
        }

        /// <summary>
        /// Removes an Item from a Basket. If the Item is not found, the same status code of 204 No Content is returned so this request idempotent.
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="itemCode"></param>
        /// <returns>204 No Content</returns>
        [HttpDelete("{itemCode}")]
        public IActionResult Delete(Guid basketId, string itemCode)
        {
            var basket = this.basketRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            this.basketRepository.RemoveItemFromBasket(basketId, itemCode);

            return NoContent();
        }

        /// <summary>
        /// Get all the Items in a Basket.
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns>200 OK and a collection of Items.</returns>
        [HttpGet]
        public IActionResult GetItemsForBasket(Guid basketId)
        {
            var basket = this.basketRepository.GetBasket(basketId);

            if (basket == null)
                return new NotFoundResult();

            return new OkObjectResult(this.basketRepository.GetItemsForBasket(basketId));
        }
    }
}
