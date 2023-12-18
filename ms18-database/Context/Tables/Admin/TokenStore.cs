namespace Maasgroep.Database.Admin
{
    public record TokenStore : GenericRecordActive
    {
        public string Token { get; set; }
        public DateTime ExperationDate { get; set; }
        
        //FK property
        public long MemberId { get; set; }
        
        //EF property
        public Member Member { get; set; }
    }
}
