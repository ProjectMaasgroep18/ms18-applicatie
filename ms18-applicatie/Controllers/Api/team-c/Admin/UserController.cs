using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Controllers.Api;

public class UserController : WritableRepositoryController<IMemberRepository, Member, MemberModel>
{
    public UserController(IMemberRepository repository) : base(repository) {}
}