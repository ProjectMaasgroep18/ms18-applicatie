using Maasgroep.Database.Receipts;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Repository.Interfaces
{
	public interface IReceiptContext
	{
		DbSet<Receipt> Receipt { get; }
		DbSet<ReceiptApproval> ReceiptApproval { get; }
		DbSet<CostCentre> CostCentre { get; }
		DbSet<Photo> Photo { get; }
	}
}
