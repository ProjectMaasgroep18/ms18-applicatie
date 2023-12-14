﻿
namespace Maasgroep.SharedKernel.ViewModels.Orders
{
	public class ProductModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		
		public string? Color { get; set; }
		
		public string? Icon { get; set; }
		
		public double Price { get; set; }
	}
}
