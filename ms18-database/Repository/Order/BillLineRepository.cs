using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Orders
{
    public class BillLineRepository : DeletableRepository<Line, LineModel, LineData>, IBillLineRepository
    {
        protected ProductRepository Products;
        public BillLineRepository(MaasgroepContext db) : base(db)
            => Products = new(db);

        /** Create LineModel from Line record */
        public override LineModel GetModel(Line Line)
        {
            return new LineModel() {
                Id = Line.Id,
                BillId = Line.BillId,
                ProductId = Line.ProductId,
                Name = Line.Name,
                Price = Line.Price,
                Quantity = Line.Quantity,
                Amount = Line.Amount,
            };
        }
        
        /** Create or update Line record from data model */
        public override Line? GetRecord(LineData data, Line? existingLine = null)
        {
            var line = existingLine ?? new();
            if (existingLine != null && existingLine.ProductId != data.ProductId)
                return null; // Line should stay with product

            var product = Products.GetById(data.ProductId);
            if (product == null)
                return null; // Line should have a product

            line.ProductId = data.ProductId;
            line.Name = product.Name;
            line.Price = product.Price;
            line.Quantity = data.Quantity;
            line.Amount = product.Price * data.Quantity;
            return line;
        }

        /** Get line record by BillId/ProductId combination */
        public virtual Line? GetByBillProduct(long billId, long productId) => Db.OrderLines.FirstOrDefault(item => item.BillId == billId && item.ProductId == productId);

        /** Get a list of line models for a specific bill */
        public virtual IEnumerable<LineModel> ListByBill(long billId, int offset = default, int limit = default, bool includeDeleted = default) =>
            GetList(line => line.BillId == billId, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}