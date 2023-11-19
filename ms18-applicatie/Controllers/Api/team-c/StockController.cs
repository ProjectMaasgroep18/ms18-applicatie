using Maasgroep.Database;
using Maasgroep.Database.Repository.ViewModel;
using Maasgroep.Database.Stock;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api.team_c
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly MaasgroepContext _context;

        public StockController(MaasgroepContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("stockGet")]
        public IActionResult StockGet()
        {
            var allStock = (from product in _context.Product
                            join stock in _context.Stock
                            on product.Id equals stock.ProductId
                            select new StockViewModel(product.Name, stock.Quantity)).ToList();

            return Ok(allStock);
        }

        [HttpGet]
        [ActionName("stockGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult StockGetById(long id)
        {
            var stockCurrent = (from product in _context.Product
                                join stock in _context.Stock
                                on product.Id equals stock.ProductId
                                where product.Id == id
                                select new StockViewModel(product.Name, stock.Quantity)).FirstOrDefault();
            //TODO: lijst opvragen, groter dan 1, etc
            return Ok(stockCurrent);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddStock(StockViewModel stock)
        {
            //if (product.Description.Contains("XYZ Widget"))
            //{
            //    return BadRequest();
            //}

            var product = _context.Product.Where(p => p.Name == stock.Name).FirstOrDefault();
            var currentStock = _context.Stock.Where(s => s.ProductId == product.Id).FirstOrDefault();

            currentStock.Quantity = stock.Quantity;
            //currentStock.MemberModifiedId = 2; // Todo

            _context.Update(currentStock);
            await _context.SaveChangesAsync();

            return Created("", product); // TODO: Aanpassen

            //return CreatedAtAction(nameof(GetById_IActionResult), new { id = product.Id }, product);
        }
    }
}
