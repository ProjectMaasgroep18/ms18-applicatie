using Maasgroep.Database.Admin;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;

namespace Maasgroep.Database.Interfaces
{
    /** Permission repository interface, connecting to Permission database records */
    public interface IPermissionRepository : IReadableRepository<Permission, PermissionModel>
    {
        PermissionModel? GetByName(string name);
    }
}
