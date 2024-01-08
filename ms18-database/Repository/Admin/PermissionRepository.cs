using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Admin
{
    public class PermissionRepository : ReadableRepository<Permission, PermissionModel>, IPermissionRepository
    {
        public PermissionRepository(MaasgroepContext db) : base(db) {}

        /** Get PermissionModel by Permission name */
        public PermissionModel? GetByName(string name)
        {
            var permission = Db.Permission.Where(p => p.Name == name).FirstOrDefault();
            if (permission == null)
                return null;
            return GetModel(permission);
        }

        /** Create PermissionModel from Permission record */
        public override PermissionModel GetModel(Permission permission)
        {
            return new PermissionModel()
            {
                Id = permission.Id,
                Name = permission.Name,
            };
        }
    }
}