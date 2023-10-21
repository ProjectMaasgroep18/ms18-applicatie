using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Models
{
    public class Receipt : IReceipt
    {
        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal? Amount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICostCentre? CostCentre { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IReceiptStatus ReceiptStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Note { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
