namespace Maasgroep.SharedKernel.ViewModels.Admin
{
    public record TokenModel
    {
        public MemberModel? Member { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
