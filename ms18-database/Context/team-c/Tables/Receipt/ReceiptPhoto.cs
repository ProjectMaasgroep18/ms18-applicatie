
namespace Maasgroep.Database.Receipts
{
    public record ReceiptPhoto : GenericRecordActive
	{
        public long ReceiptId { get; set; }
        public string Base64Image { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }

        
        //Ef
        public Receipt Receipt { get; set; }  
                
    }
}
