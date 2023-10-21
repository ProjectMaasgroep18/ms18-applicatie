using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Photos
{
    internal record Photo
    {
        public long Id { get; set; }
        public long? Receipt { get; set; }
        public byte[] Bytes { get; set; }
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
    }
}
