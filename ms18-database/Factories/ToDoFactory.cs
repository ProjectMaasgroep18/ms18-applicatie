using Microsoft.EntityFrameworkCore.Design;

namespace Maasgroep.Database.Factories
{
	public class ToDoContextFactory : IDesignTimeDbContextFactory<ToDoContext>
	{
		public ToDoContext CreateDbContext(string[] args)
		{
			return new ToDoContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
		}
	}
}
