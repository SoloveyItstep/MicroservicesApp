using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class CatalogController: ControllerBase
    {
        readonly IProductRepository repository;
        readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)] // resturn only type IEnumerable<Product> and with status OK
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() => Ok(await repository.GetProducts().ConfigureAwait(false));

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await repository.GetProduct(id).ConfigureAwait(false);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet, Route("[action]/{category}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category) => 
            Ok(await repository.GetproductsByCategoryName(category).ConfigureAwait(false));

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
            await repository.Create(product).ConfigureAwait(false);
            return CreatedAtRoute("GetProduct", new { id = product.Id });
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product) => Ok(await repository.Update(product).ConfigureAwait(false));

        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id) => Ok(await repository.Delete(id).ConfigureAwait(false));
    }
}
