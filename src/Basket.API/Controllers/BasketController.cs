using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repository;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        readonly IBasketRepository basketRepository;
        readonly IMapper mapper;
        readonly EventRabbitMQProducer eventBus;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, EventRabbitMQProducer eventBus)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
            this.eventBus = eventBus;
        }

        [HttpGet, ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await basketRepository.GetBasket(userName).ConfigureAwait(false);
            return Ok(basket?? new BasketCart(userName));
        }

        [HttpPost, ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket)
        {
            return Ok(await basketRepository.UpdateBasket(basket).ConfigureAwait(false));
        }

        [HttpDelete("{userName}"), ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await basketRepository.DeleteBasket(userName).ConfigureAwait(false));
        }

        [HttpPost, Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted), ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody]BasketCheckout basketCheckout)
        {
            var basket = await basketRepository.GetBasket(basketCheckout.UserName).ConfigureAwait(false);
            if (basket == null)
                return BadRequest();

            var basketRemoved = await basketRepository.DeleteBasket(basket.UserName).ConfigureAwait(false);
            if (!basketRemoved)
                return BadRequest();

            var eventMessage = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch(Exception e)
            {
                throw;
            }

            return Accepted();
        }
    }
}
