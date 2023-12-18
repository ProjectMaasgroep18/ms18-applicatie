namespace Maasgroep.SharedKernel.ViewModels.Orders
{
	public record LineModel
	{
		public long Id { get; set; }
		public long BillId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public double Amount { get; set; }
	}
}
