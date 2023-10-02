
namespace Maasgroep.Database.Members
{
    internal record Permission
    {
        public long Id { get; set; }
        public string Name { get; set; }


        // Generic
        public long UserCreatedId { get; set; }
        public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Members { get; set; }


        // EF generic properties
        public Member UserCreated { get; set; }
        public Member? UserModified { get; set; }


    }
}
