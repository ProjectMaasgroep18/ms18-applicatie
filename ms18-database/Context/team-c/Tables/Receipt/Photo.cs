using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record Photo : GenericRecordActive
    {
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public string Base64Image { get; set; }
        public string fileExtension { get; set; }
        public string fileName { get; set; }
        public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?

        
        //Ef
        public Receipt Receipt { get; set; }  
        
        public static Photo FromViewModel(PhotoViewModel viewModel)
        {
            return new Photo
            {
                Id = viewModel.Id,
                ReceiptId = viewModel.ReceiptId,
                fileExtension = viewModel.fileExtension,
                fileName = viewModel.fileName,
                DateTimeCreated = viewModel.DateTimeCreated,
                DateTimeModified = viewModel.DateTimeModified,
                Base64Image = viewModel.Base64Image,
            };
        }
        
    }
}
