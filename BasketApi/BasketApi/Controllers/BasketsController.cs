using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BasketApi.Controllers
{
    [Route("api/[controller]")]
    public class BasketsController : Controller
    {
        private readonly IBasketsRepository basketsRepository;

        public BasketsController(IBasketsRepository basketsRepository)
        {
            this.basketsRepository = basketsRepository;
        }

        /// <summary>
        /// Creates a new Basket.
        /// </summary>
        /// <returns>The newly created Basket.</returns>
        [HttpPost]
        public IActionResult Post()
        {
            var newBasket = this.basketsRepository.CreateBasket();

            return CreatedAtRoute("GetBasket", new { basketId = newBasket.Id }, newBasket);
        }

        /// <summary>
        /// Clears all the Items froma Basket.
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns>204 No Content</returns>
        [HttpPost("{basketId}/clear")]
        public IActionResult Clear(Guid basketId)
        {
            var basket = this.basketsRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            this.basketsRepository.ClearBasket(basketId);

            return NoContent();
        }

        /// <summary>
        /// Gets a Basket.
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns>Gets a Basket and its Items.</returns>
        [HttpGet("{basketId}", Name="GetBasket")]
        public IActionResult Get(Guid basketId)
        {
            var basket = this.basketsRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }
    }
}
