using Maasgroep.Database.Admin;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;

namespace Maasgroep.Database.Interfaces
{
    /** Member repository interface, connecting to Member database records */
    public interface IMemberRepository : IEditableRepository<Member, MemberModel, MemberData, MemberHistory>
    {
        MemberModel? GetByEmail(string email, string password);
        string GetPasswordHash(string password);
        bool CheckPassword(string password, string hash);
        bool CheckPassword(string password, long memberId);
    }
}
