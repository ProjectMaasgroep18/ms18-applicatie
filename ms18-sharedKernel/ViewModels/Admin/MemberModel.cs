﻿
namespace Maasgroep.SharedKernel.ViewModels.Admin
{
    public record MemberModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Color { get; set; }
        public bool IsGuest { get; set; }
        public List<string> Permissions { get; set; }
    }
}
