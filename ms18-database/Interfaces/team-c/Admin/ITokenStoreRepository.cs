using Maasgroep.Database.Admin;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Interfaces
{
    public interface ITokenStoreRepository : IReadOnlyRepository<TokenStore, TokenModel>
    {
        bool SaveToken(string token, DateTime expire, long userId);
        long? GetMemberIdFromToken(string token);
    }
}
