namespace Maasgroep.SharedKernel.DataModels.Admin
{
    public record MemberCredentialsData: MemberData
    {
        // CurrentPassword MUST be the current user's password (which may be different from the user being edited)
        // Set values that should not be changed to "null"
        public string CurrentPassword { get; set; }
        public string? NewEmail { get; set; }
        public string? NewPassword { get; set; }
        public string[]? NewPermissions { get; set; }
    }
}
