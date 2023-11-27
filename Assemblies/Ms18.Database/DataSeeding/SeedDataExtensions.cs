using Microsoft.EntityFrameworkCore;
using Ms18.Database.DataSeeding.Seeders;

namespace Ms18.Database.DataSeeding;
public static class SeedDataExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        MemberSeeder.Seed(modelBuilder);
        //PermissionSeeder.Seed(modelBuilder);
        //MemberPermissionSeeder.Seed(modelBuilder);
    }
}