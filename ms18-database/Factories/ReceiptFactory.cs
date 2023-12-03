using Microsoft.EntityFrameworkCore.Design;

namespace Maasgroep.Database.Factories
{
	public class ReceiptFactory : IDesignTimeDbContextFactory<ReceiptContext>
	{
		public ReceiptContext CreateDbContext(string[] args)
		{
			return new ReceiptContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}
	}
}
