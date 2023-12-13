
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class CostCentreModel : IEquatable<CostCentreModel>
	{
		public CostCentreModel() { }

		public long Id { get; set; }
		public string Name { get; set; }

		public bool Equals(CostCentreModel? other)
		{
			return Name == other.Name;
		}
	}
}
