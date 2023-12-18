
namespace Maasgroep.SharedKernel.ViewModels.Orders
{
	public class ProductModel : IEquatable<ProductModel>
	{
		public long Id { get; set; }
		public string Name { get; set; }
		
		public string? Color { get; set; }
		
		public string? Icon { get; set; }
		
		public double Price { get; set; }

		public bool Equals(ProductModel? other)
		{

			if (Name != other.Name)
			{
				return false;
			}
			
			if(Icon != other.Icon)
			{
				return false;
			}
			
			if(Color != other.Color)
			{
				return false;
			}
			
			if(Price != other.Price)
			{
				return false;
			}
			
			return true;
		}
	}
}
