using Ms18.Database.Models.TeamC.Admin;
using Ms18.Database.Repository.TeamC.ViewModel;

namespace Ms18.Database.Models.TeamC.Receipt
{
    public record ReceiptPhoto
    {
        public long Id { get; set; }
        public long? Receipt { get; set; }

        [Obsolete("Use Base64Image instead", false)] // Not marked as error because this will break the build
        public byte[]? Bytes { get; set; }

        public string Base64Image { get; set; }

        public string fileExtension { get; set; }
        public string fileName { get; set; }
        public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }


        //Ef
        public Receipt? ReceiptInstance { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

        public static ReceiptPhoto FromViewModel(PhotoViewModel viewModel)
        {
            return new ReceiptPhoto
            {
                Receipt = viewModel.Receipt,
                fileExtension = viewModel.fileExtension,
                fileName = viewModel.fileName,
                Base64Image = viewModel.Base64Image,
            };
        }

    }
}
