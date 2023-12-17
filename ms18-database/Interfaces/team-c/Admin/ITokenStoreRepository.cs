using Maasgroep.SharedKernel.ViewModels.team_c.Authentication;

namespace Maasgroep.Database.Interfaces
{
    public interface ITokenStoreRepository
    {
        TokenModel RetrieveToken(long id);
        TokenModelCreate SaveToken(TokenModelCreate token);
        TokenModelUpdate UpdateUserToken(TokenModelUpdate token, long userId);
        long? GetMemberFromToken(string token);
    }
}
