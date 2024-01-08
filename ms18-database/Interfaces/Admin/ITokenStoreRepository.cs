using Maasgroep.Database.Admin;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Interfaces
{
    /** TokenStore repository, used to store member session tokens in the database */
    public interface ITokenStoreRepository : IReadableRepository<TokenStore, TokenModel>
    {
        bool SaveToken(string token, DateTime expire, long memberId);
        bool DeleteToken(string token, long memberId);
        long? GetMemberIdFromToken(string token);
    }
}
