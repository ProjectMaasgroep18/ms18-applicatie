﻿
namespace Maasgroep.SharedKernel.ViewModels.Orders
{
	public class ProductModel : IEquatable<ProductModel>
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public bool Equals(ProductModel? other)
		{
			return Name == other.Name;
		}
	}
}