using Microsoft.EntityFrameworkCore.Design;

namespace Maasgroep.Database.Factories
{
	public class MemberContextFactory : IDesignTimeDbContextFactory<MemberContext>
	{
		public MemberContext CreateDbContext(string[] args)
		{
			return new MemberContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}
	}
}
