
namespace Maasgroep.SharedKernel.ViewModels.Orders
{
	public record ProductModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string? Color { get; set; }
		public string? Icon { get; set; }
		public double Price { get; set; }
		public long PriceQuantity { get; set; }
		public long Stock { get; set; }
	}
}
