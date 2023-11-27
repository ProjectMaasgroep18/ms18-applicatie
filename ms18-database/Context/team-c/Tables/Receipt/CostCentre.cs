using Maasgroep.Database.Members;
using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record CostCentre : GenericRecordActive
	{
		public long Id { get; set; }
		public string Name { get; set; }


        // EF receipt properties
        public ICollection<Receipt> Receipt { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

        public static CostCentre FromViewModel(CostCentreViewModel viewModel)
        {
            return new CostCentre
            {
                Name = viewModel.Name
            };
        }
    }
}
