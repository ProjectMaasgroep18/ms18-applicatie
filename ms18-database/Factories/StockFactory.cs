using Microsoft.EntityFrameworkCore.Design;

namespace Maasgroep.Database.Factories
{
	public class StockContextFactory : IDesignTimeDbContextFactory<StockContext>
	{
		public StockContext CreateDbContext(string[] args)
		{
			return new StockContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}
	}
}
