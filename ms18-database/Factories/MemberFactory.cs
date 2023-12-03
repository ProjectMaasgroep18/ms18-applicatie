using Microsoft.EntityFrameworkCore.Design;

namespace Maasgroep.Database.Factories
{
	public class MaasgroepContextFactory : IDesignTimeDbContextFactory<MaasgroepContext>
	{
		public MaasgroepContext CreateDbContext(string[] args)
		{
			return new MaasgroepContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}
	}
}
