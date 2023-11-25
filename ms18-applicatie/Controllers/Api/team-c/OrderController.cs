using Maasgroep.Database;
using Maasgroep.Database.Repository.ViewModel;
using Maasgroep.Database.Order;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api.team_c
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MaasgroepContext _context;

        public OrderController(MaasgroepContext context)
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
        public IActionResult ModifyStock(StockViewModel stock)
        {
            if (stock.Quantity < 0)
            {
                return BadRequest("Quantity must be equal or greater than 0.");
            }

            // TODO: Dit zit niet lekker in elkaar. Misschien controles anders, misschien model anders.

            var product = _context.Product.Where(p => p.Name == stock.Name).FirstOrDefault();

            if (product == null)
            {
                return BadRequest("Dit moet nog gedaan worden, toevoegen");
            }
            else
            {
                var currentStock = _context.Stock.Where(s => s.ProductId == product.Id).FirstOrDefault();

                if (currentStock == null) 
                {
                    return BadRequest("Dit moet nog gedaan worden, toevoegen");
                }
                else
                {
                    var currentStockToHistory = new StockpileHistory(currentStock);

                    _context.StockHistory.Add(currentStockToHistory);

                    currentStock.Quantity = stock.Quantity;
                    currentStock.MemberModifiedId = _context.Member.Where(x => x.Id == 2).FirstOrDefault().Id;
                    currentStock.DateTimeModified = DateTime.UtcNow;

                    _context.Update(currentStock);
                    _context.SaveChanges();

                    return Created("", product); // TODO: Aanpassen
                                                 //return CreatedAtAction(nameof(GetById_IActionResult), new { id = product.Id }, product);
                }
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteStock(StockViewModel stock)
        {
            return Ok("hoi");
        }
    }
}
