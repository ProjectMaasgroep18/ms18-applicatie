using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{
    public class CostCentreRepository : EditableRepository<CostCentre, CostCentreModel, CostCentreData, CostCentreHistory>, ICostCentreRepository
    {
        public CostCentreRepository(MaasgroepContext db) : base(db) {}

        /** Create CostCentreModel from CostCentre record */
        public override CostCentreModel GetModel(CostCentre costCentre)
        {
            return new CostCentreModel() {
				Id = costCentre.Id,
				Name = costCentre.Name,
			};
        }

		/** Create or update CostCentre record from data model */
        public override CostCentre? GetRecord(CostCentreData data, CostCentre? existingCostCentre = null)
        {
            var costCentre = existingCostCentre ?? new();
			costCentre.Name = data.Name;
			return costCentre;
        }

		/** Create a CostCentreHistory record from a CostCentre record */
        public override CostCentreHistory GetHistory(CostCentre costCentre)
        {
            return new CostCentreHistory() {
				CostCentreId = costCentre.Id,
				Name = costCentre.Name,
			};
        }
    }
}
