
namespace Maasgroep.Database.Interfaces
{
    public interface IReceipt
    {
        long Id { get; set; }
        decimal? Amount { get; set; }
        ICostCentre? CostCentre { get; set; }
        IReceiptStatus ReceiptStatus { get; set; }
        string? Location { get; set; }
        string? Note { get; set; }
    }
}
