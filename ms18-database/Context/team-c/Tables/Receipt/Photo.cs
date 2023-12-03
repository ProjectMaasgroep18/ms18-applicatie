
namespace Maasgroep.Database.Receipts
{
    public record Photo : ReceiptActiveRecord
	{
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public string Base64Image { get; set; }
        public string fileExtension { get; set; }
        public string fileName { get; set; }
        public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?

        
        //Ef
        public Receipt Receipt { get; set; }  
                
    }
}
