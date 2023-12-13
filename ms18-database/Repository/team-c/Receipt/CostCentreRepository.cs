using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class CostCentreRepository : EditableRepository<CostCentre, CostCentreModel, CostCentreHistory>, ICostCentreRepository<CostCentre, CostCentreHistory>
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

		/** Create or update CostCentre record from model */
        public override CostCentre? GetRecord(CostCentreModel model, CostCentre? existingCostCentre = null)
        {
            var costCentre = existingCostCentre ?? new();
			costCentre.Name = model.Name;
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
