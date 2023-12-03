
namespace Maasgroep.Database.Members
{
    public record Member : MemberRecordActive
	{
        public long Id { get; set; }
        public string Name { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Permissions { get; set; }

        
        // EF Generic properties
        public ICollection<Member>? MembersCreated { get; set; }
        public ICollection<Member>? MembersModified { get; set; }
        public ICollection<Member>? MembersDeleted { get; set; }
        public ICollection<Permission> PermissionsCreated { get; set; }
        public ICollection<Permission> PermissionsModified { get; set; }
        public ICollection<Permission> PermissionsDeleted { get; set; }
        public ICollection<MemberPermission> MemberPermissionsCreated { get; set; }
        public ICollection<MemberPermission> MemberPermissionsModified { get; set; }
        public ICollection<MemberPermission> MemberPermissionsDeleted { get; set; }

    }
}
