using Microsoft.EntityFrameworkCore;
using Ms18.Database.Models.TeamC.Admin;

namespace Ms18.Database.DataSeeding.Seeders;

public static class MemberSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var members = new List<Member>
        {
            new() { Id = 1, Name = "Borgia", MemberCreatedId = 1, MemberModifiedId = 1, DateTimeCreated = DateTime.UtcNow},
            new() { Id = 2, Name = "da Gama", MemberCreatedId = 1, DateTimeCreated = DateTime.UtcNow },
            new() { Id = 3, Name = "Albuquerque", MemberCreatedId = 1, DateTimeCreated = DateTime.UtcNow }
        };

        modelBuilder.Entity<Member>().HasData(members);

        var permissions = new List<Permission>
        {
            new() { Id = 1, Name = "receipt.approve", MemberCreatedId = 1 },
            new() { Id = 2, Name = "receipt.reject", MemberCreatedId = 1 },
            new() { Id = 3, Name = "receipt.handIn", MemberCreatedId = 1 },
            new() { Id = 4, Name = "receipt.payOut", MemberCreatedId = 1 }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);

        var memberPermissions = new List<MemberPermission>
        {
            new() { MemberId = 2, PermissionId = 1, MemberCreatedId = 1 },
            new() { MemberId = 2, PermissionId = 2, MemberCreatedId = 1 },
            new() { MemberId = 3, PermissionId = 3, MemberCreatedId = 1 },
        };

        modelBuilder.Entity<MemberPermission>().HasData(memberPermissions);
    }
}

