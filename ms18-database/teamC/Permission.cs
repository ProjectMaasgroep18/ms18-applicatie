using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
    public record Permission
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }

        //Generic
        public long UserCreatedId { get; set; }
        public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }

        // Ef admin properties
        public ICollection<MemberPermission> MemberPermissions { get; set; }
        public ICollection<Member> Members { get; set; }

        // Ef generic
        public Member UserCreated { get; set; }
        public Member? UserModified { get; set; }


    }
}
