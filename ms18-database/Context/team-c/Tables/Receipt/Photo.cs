
namespace Maasgroep.Database.Receipts
{
    public record Photo : GenericRecordActive
	{
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public string Base64Image { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?

        
        //Ef
        public Receipt Receipt { get; set; }  
                
    }
}
