using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Orders
{
    public class BillRepository : DeletableRepository<Bill, BillModel, BillData>, IBillRepository
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
                Lines = Lines.ListByBill(bill.Id, 0, Int32.MaxValue).Where(line => line.Quantity > 0).ToList(),
                TotalAmount = bill.TotalAmount,
            };
        }
        
        /** Create or update Bill record from data model */
        public override Bill? GetRecord(BillData data, Bill? existingBill = null)
        {
            if (existingBill != null)
                return null;

            var lines = new Dictionary<long, LineData>();

            foreach (var line in data.Lines) {
                // Build lines from data model

                if (!lines.ContainsKey(line.ProductId)) {
                    // There is no line for this product yet

                    lines.Add(line.ProductId, new () {
                        ProductId = line.ProductId,
                        Quantity = 0,
                    });
                }
                
                // Increment quantity in Data model (this way duplicate lines are merged)
                lines[line.ProductId].Quantity += line.Quantity;
            }

            var bill = new Bill
            {
                Name = data.Name,
                Note = data.Note,
                Lines = lines.ToList()
                    .Select(line => Lines.GetRecord(line.Value))
                    .Where(line => line != null)
                    .ToList()!
            };
            if (bill.Lines.Count == 0)
                return null; // There must be at least one line in the bill
            bill.TotalAmount = bill.Lines.Sum(line => line.Amount);
            return bill;
        }
    }
}