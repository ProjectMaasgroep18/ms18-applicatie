
namespace Maasgroep.Database.Admin
{
    public record Permission : GenericRecordActive
	{
        public long Id { get; set; }
        public string Name { get; set; }


        // EF admin properties
        public ICollection<MemberPermission> Members { get; set; }

    }
}
