
namespace Maasgroep.Database.Receipts
{
    public record PhotoHistory : GenericRecordHistory
    {
        public long PhotoId { get; set; }
        public long ReceiptId { get; set; }
        public string Base64Image { get; set; }
        public string fileExtension { get; set; }
        public string fileName { get; set; }
        public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?
    }
}
