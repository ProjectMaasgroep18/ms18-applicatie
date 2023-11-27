namespace Ms18.Database.Models.TeamC.Admin
{
    public record MemberPermission
    {
        public long MemberId { get; set; }
        public long PermissionId { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }


        // EF admin properties
        public Member Member { get; set; }
        public Permission Permission { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

    }
}
