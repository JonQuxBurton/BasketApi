﻿using BasketApi.Domain;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post()
        {
            var newBasket = this.basketsRepository.CreateBasket();

            return new OkObjectResult(newBasket);
        }

        [HttpPost("{basketId}/clear")]
        public IActionResult Clear(Guid basketId)
        {
            var basket = this.basketsRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            this.basketsRepository.ClearBasket(basketId);

            return NoContent();
        }

        [HttpGet("{basketId}")]
        public IActionResult Get(Guid basketId)
        {
            var basket = this.basketsRepository.GetBasket(basketId);

            if (basket == null)
                return NotFound();

            return new OkObjectResult(basket);
        }
    }
}