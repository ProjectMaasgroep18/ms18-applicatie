using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.SharedKernel.ViewModels.Order;

namespace Maasgroep.Database.Orders
{
	public class OrderRepository : IOrderRepository
	{
		private readonly MaasgroepContext _orderContext;

		public OrderRepository(MaasgroepContext orderContext)
		{
			_orderContext = orderContext;
		}

		public long Add(ProductModelCreateDb product)
		{
			throw new NotImplementedException();
		}

		public long Add(StockModelCreateDb product)
		{
			throw new NotImplementedException();
		}

		public bool Delete(ProductModel product)
		{
			throw new NotImplementedException();
		}

		public bool Delete(StockModel product)
		{
			throw new NotImplementedException();
		}

		public ProductModel GetProduct(long id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ProductModel> GetProducts(int offset, int fetch)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<StockModel> GetStock(int offset, int fetch)
		{
			throw new NotImplementedException();
		}

		public StockModel GetStock(long id)
		{
			throw new NotImplementedException();
		}

		public bool Modify(ProductModelUpdateDb product)
		{
			throw new NotImplementedException();
		}

		public bool Modify(StockModelUpdateDb product)
		{
			throw new NotImplementedException();
		}
	}
}
