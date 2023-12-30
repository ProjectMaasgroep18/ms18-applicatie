namespace Maasgroep.SharedKernel.DataModels.Admin
{
    public record MemberData
    {
        public string Name { get; set; }
        public string? Email { get; set; } // New member only
        public string? Color { get; set; }
    }
}
