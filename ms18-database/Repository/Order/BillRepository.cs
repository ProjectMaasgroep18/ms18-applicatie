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
                Email = bill.Email,
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
                Email = data.Email,
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

        /** Ensure IsGuest is set correctly when saving bill */
        public override Action<MaasgroepContext> GetSaveAction(Bill record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                var member = record.MemberCreated;
                if (member == null && record.MemberCreatedId != null)
                    member = db.Member.Where(m => m.Id == record.MemberCreatedId).FirstOrDefault();
                if (record.Name == null || record.Name == "")
                    record.Name = member?.Name;
                if (record.Email == null || record.Email == "")
                    record.Email = member?.Email;
                record.IsGuest = record.MemberCreatedId == null || (member?.IsGuest ?? true);
                saveAction.Invoke(db);
            };
        }

        /** Get total number and value of bills, for all users or for a specific member */
        public BillTotalModel GetTotal(long? MemberId = null)
        {
            var billData = Db.Bills
                .Where(b => b.DateTimeDeleted == null && (MemberId == null || b.MemberCreatedId == MemberId))
                .Select(b => new { Quantity = b.Lines.Sum(l => l.Quantity), Amount = b.TotalAmount })
                .ToList();

            var totals = new BillTotalModel()
            {
                BillCount = billData.Count,
                ProductQuantity = billData.Sum(bd => bd.Quantity),
                TotalAmount = billData.Sum(bd => bd.Amount),
            };

            return totals;
        }

        // List bills by e-mail
        public IEnumerable<BillModel> ListByEmail(string email, int offset = 0, int limit = 0, bool includeDeleted = false)
            => GetList(item => item.Email == email, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}