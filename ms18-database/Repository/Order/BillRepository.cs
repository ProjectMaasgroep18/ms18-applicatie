using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Orders
{
    public class BillRepository : EditableRepository<Bill, BillModel, BillData, BillHistory>, IBillRepository
    {
        protected BillLineRepository Lines;
        protected ProductRepository Products;
        protected StockRepository Stock;
        protected MemberRepository Members;

        public BillRepository(MaasgroepContext db) : base(db)
        {
            Lines = new(db);
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
                Lines = Lines.ListByBill(bill.Id).Where(line => line.Quantity > 0).ToList(),
                TotalAmount = bill.TotalAmount,
			};
        }
        
        /** Create or update Bill record from data model */
        public override Bill? GetRecord(BillData data, Bill? existingBill = null)
        {
            var lines = new Dictionary<long, LineData>();
            if (existingBill != null)
            {
                // Create existing lines with 0 quantity
                Lines.ListByBill(existingBill.Id).ToList().ForEach(line => {
                    if (!lines.ContainsKey(line.ProductId))
                    {
                        lines.Add(line.ProductId, new () {
                            ProductId = line.ProductId,
                            Quantity = 0,
                        });
                    }
                });
            }
   
            data.Lines.ForEach(line => {
                // Merge with new lines
                if (lines.ContainsKey(line.ProductId))
                {
                    lines[line.ProductId].Quantity += line.Quantity;
                }
                else
                {
                    lines.Add(line.ProductId, new () {
                        ProductId = line.ProductId,
                        Quantity = line.Quantity,
                    });;
                }
            });

            var bill = existingBill ?? new();
			bill.Name = data.Name;
			bill.Note = data.Note;
            bill.Lines = lines.ToList()
                .Select(line => Lines.GetRecord(line.Value, existingBill != null ? Lines.GetByBillProduct(existingBill.Id, line.Key) : null))
                .Where(line => line != null).ToList()!;
            bill.TotalAmount = bill.Lines.Sum(line => line.Amount);
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
                TotalAmount = bill.TotalAmount,
			};
        }

        /** Ensure lines are created and IsGuest is set correctly when saving bill */
        public override Action<MaasgroepContext> GetSaveAction(Bill record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                record.IsGuest = record.MemberCreatedId == null;

                saveAction.Invoke(db);
            };
        }
    }
}