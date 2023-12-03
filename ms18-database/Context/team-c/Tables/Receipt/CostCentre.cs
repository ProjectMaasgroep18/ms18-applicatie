using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record CostCentre : ReceiptActiveRecord
	{
		public long Id { get; set; }
		public string Name { get; set; }


        // EF receipt properties
        public ICollection<Receipt> Receipt { get; set; }

        public static CostCentre FromViewModel(CostCentreViewModel viewModel)
        {
            return new CostCentre
            {
                Name = viewModel.Name
            };
        }
    }
}
