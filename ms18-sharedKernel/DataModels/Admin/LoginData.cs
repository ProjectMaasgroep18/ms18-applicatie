namespace Maasgroep.SharedKernel.DataModels.Admin
{
    public record LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
