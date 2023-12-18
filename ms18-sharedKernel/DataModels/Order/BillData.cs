namespace Maasgroep.SharedKernel.DataModels.Orders
{
	public record BillData
	{
        public List<LineData> Lines { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
	}
}
