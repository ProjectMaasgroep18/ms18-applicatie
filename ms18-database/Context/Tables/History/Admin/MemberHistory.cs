namespace Maasgroep.Database.Admin
{
    public record MemberHistory : GenericRecordHistory
    {
        public long MemberId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Color { get; set; }
        public bool IsGuest { get; set; }
        public string MemberPermissions { get; set; }
    }
}