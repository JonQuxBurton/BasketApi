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

        [HttpDelete("{itemCode}")]
        public IActionResult Delete(Guid basketId, string itemCode)
        {
            var basket = this.basketRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            this.basketRepository.RemoveItemFromBasket(basketId, itemCode);

            return NoContent();
        }

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
