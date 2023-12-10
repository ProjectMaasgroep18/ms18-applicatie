using Microsoft.EntityFrameworkCore;
using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.SharedKernel.ViewModels.Orders;

namespace Maasgroep.Database.Orders
{
	public class OrderRepository : IOrderRepository
	{
		private readonly MaasgroepContext _orderContext;

		public OrderRepository(MaasgroepContext orderContext)
		{
			_orderContext = orderContext;
		}

		#region Product

		public ProductModel GetProduct(long id)
		{
			var product = _orderContext.Product.Where(p => p.Id == id).FirstOrDefault();

			var result = new ProductModel()
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				Color = product.Color,
				Icon = product.Icon
			};

			return result;
		}

		public IEnumerable<ProductModel> GetProducts(int offset, int fetch)
		{
			var result = new List<ProductModel>();

			var products = _orderContext.Product.ToList();

			foreach (var product in products)
				result.Add(GetProduct(product.Id));
			
			return result.OrderBy(p => p.Id).ToList();
		}

		public long Add(ProductModelCreateDb productToCreate)
		{
			var productToAdd = new Product()
			{
				MemberCreatedId = productToCreate.Member.Id,
				Name = productToCreate.Product.Name,
				Price = productToCreate.Product.Price ?? 0,
				Color = productToCreate.Product.Color,
				Icon = productToCreate.Product.Icon,
			};

			_orderContext.Database.BeginTransaction();

			_orderContext.Product.Add(productToAdd);
			_orderContext.SaveChanges();

			var idOfProduct = _orderContext.Database.SqlQuery<long>($"select currval('order.\"productSeq\"'::regclass)").ToList().FirstOrDefault();

			_orderContext.Database.CommitTransaction();

			return idOfProduct;
		}

		public bool Modify(ProductModelUpdateDb productToUpdate)
		{
			var product = _orderContext.Product.Where(r => r.Id == productToUpdate.Product.Id).FirstOrDefault();

			if (product == null) throw new Exception("kapot!");

			_orderContext.Database.BeginTransaction();
			_orderContext.ProductHistory.Add(CreateProductHistory(product));
			_orderContext.SaveChanges();

			product.Name = productToUpdate.Product.Name;
			product.Color = productToUpdate.Product.Color;
			product.Icon = productToUpdate.Product.Icon;
			product.Price = productToUpdate.Product.Price;
			
			
			product.MemberModifiedId = productToUpdate.Member.Id;
			product.DateTimeModified = DateTime.UtcNow;

			_orderContext.Update(product);
			_orderContext.SaveChanges();
			_orderContext.Database.CommitTransaction();

			return true;
		}

		public bool Delete(ProductModelDeleteDb productToDelete)
		{
			var product = _orderContext.Product.Where(p => p.Id == productToDelete.Product.Id).FirstOrDefault();

			_orderContext.Database.BeginTransaction();

			_orderContext.ProductHistory.Add(CreateProductHistory(product));
			_orderContext.SaveChanges();

			product.DateTimeDeleted = DateTime.UtcNow;
			product.MemberDeletedId = productToDelete.Member.Id;
			_orderContext.Product.Update(product);
			_orderContext.SaveChanges();

			_orderContext.Database.CommitTransaction();

			return true;
		}

		#endregion

		#region Stock

		public IEnumerable<StockModel> GetStock(int offset, int fetch)
		{
			var stock = _orderContext.Stock.ToList();

			var result = new List<StockModel>();

			foreach (var item in stock)
				result.Add(GetStock(item.ProductId));

			return result;
		}

		public StockModel GetStock(long id)
		{
			var stock = _orderContext.Stock.Where(p => p.ProductId == id).FirstOrDefault();
			var product = GetProduct(id);

			var result = new StockModel()
			{
				Product = product,
				Quantity = stock.Quantity
			};

			return result;
		}

		public long Add(StockModelCreateDb stockToCreate)
		{
			var stockToAdd = new Stock()
			{
				MemberCreatedId = stockToCreate.Member.Id,
				Quantity = stockToCreate.Stock.Quantity,
				ProductId = stockToCreate.Stock.ProductId
			};

			_orderContext.Database.BeginTransaction();

			_orderContext.Stock.Add(stockToAdd);
			_orderContext.SaveChanges();

			var idOfProduct = _orderContext.Database.SqlQuery<long>($"select currval('order.\"stockSeq\"'::regclass)").ToList().FirstOrDefault();

			_orderContext.Database.CommitTransaction();

			return idOfProduct;
		}

		public bool Modify(StockModelUpdateDb stockToUpdate)
		{
			var stock = _orderContext.Stock.Where(s => s.ProductId == stockToUpdate.Stock.Product.Id).FirstOrDefault();

			if (stock == null) throw new Exception("kapot!");

			_orderContext.Database.BeginTransaction();
			_orderContext.StockHistory.Add(CreateStockHistory(stock));
			_orderContext.SaveChanges();

			stock.Quantity = stockToUpdate.Stock.Quantity;
			stock.MemberModifiedId = stockToUpdate.Member.Id;
			stock.DateTimeModified = DateTime.UtcNow;

			_orderContext.Update(stock);
			_orderContext.SaveChanges();
			_orderContext.Database.CommitTransaction();

			return true;
		}

		public bool Delete(StockModelDeleteDb stockToDelete)
		{
			var stock = _orderContext.Stock.Where(s => s.ProductId == stockToDelete.Stock.Product.Id).FirstOrDefault();

			_orderContext.Database.BeginTransaction();

			_orderContext.StockHistory.Add(CreateStockHistory(stock));
			_orderContext.SaveChanges();

			stock.DateTimeDeleted = DateTime.UtcNow;
			stock.MemberDeletedId = stockToDelete.Member.Id;
			_orderContext.Stock.Update(stock);
			_orderContext.SaveChanges();

			_orderContext.Database.CommitTransaction();

			return true;
		}

		#endregion

		private ProductHistory CreateProductHistory(Product product)
		{
			var history = new ProductHistory();

			history.ProductId = product.Id;
			history.Name = product.Name;

			history.MemberCreatedId = product.MemberCreatedId;
			history.MemberModifiedId = product.MemberModifiedId;
			history.MemberDeletedId = product.MemberDeletedId;
			history.DateTimeCreated = product.DateTimeCreated;
			history.DateTimeModified = product.DateTimeModified;
			history.DateTimeDeleted = product.DateTimeDeleted;

			return history;
		}

		private StockHistory CreateStockHistory(Stock stock)
		{
			var history = new StockHistory();

			history.ProductId = stock.ProductId;
			history.Quantity = stock.Quantity;

			history.MemberCreatedId = stock.MemberCreatedId;
			history.MemberModifiedId = stock.MemberModifiedId;
			history.MemberDeletedId = stock.MemberDeletedId;
			history.DateTimeCreated = stock.DateTimeCreated;
			history.DateTimeModified = stock.DateTimeModified;
			history.DateTimeDeleted = stock.DateTimeDeleted;

			return history;
		}
	}
}
