using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Orders
{
    public class BillRepository : EditableRepository<Bill, BillModel, BillData, BillHistory>, IBillRepository
    {
        protected ProductRepository Products;
        protected StockRepository Stock;
        protected MemberRepository Members;
        public BillRepository(MaasgroepContext db) : base(db)
        {
            // BillLines = new(db);
            Products = new(db);
            Stock = new(db);
            Members = new(db);
        }

        /** Create BillModel from Bill record */
        public override BillModel GetModel(Bill bill)
        {
            var member = bill.MemberCreatedId != null ? Members.GetModel((long)bill.MemberCreatedId) : null;
            return new BillModel() {
				Id = bill.Id,
				Name = bill.Name,
				Note = bill.Note,
                IsGuest = bill.IsGuest,
                MemberCreated = member,
                //Lines = BillLines.GetByBill(bill.Id),
			};
        }
        
        /** Create or update Bill record from data model */
        public override Bill? GetRecord(BillData data, Bill? existingBill = null)
        {
            var bill = existingBill ?? new();
			bill.Name = data.Name;
			bill.Note = data.Note;
			return bill;
        }
        
        /** Create a BillHistory record from a Bill record */
        public override BillHistory GetHistory(Bill bill)
        {
            return new BillHistory() {
				Id = bill.Id,
				Name = bill.Name,
				Note = bill.Note,
				IsGuest = bill.IsGuest,
			};
        }

        /** Ensure lines are created and IsGuest is set correctly when saving bill */
        public override Action<MaasgroepContext> GetSaveAction(Bill record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                record.IsGuest = record.MemberCreatedId == null;
                saveAction.Invoke(db);
                /////// CREATE LINES?? (or auto based on getRecord????)
            };
        }
    }
}