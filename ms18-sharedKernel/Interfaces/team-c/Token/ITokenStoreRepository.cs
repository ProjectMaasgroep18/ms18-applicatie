using Maasgroep.SharedKernel.ViewModels.team_c.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maasgroep.SharedKernel.Interfaces.Token
{
    public interface ITokenStoreRepository
    {
        TokenModel RetrieveToken(long id);
        TokenModelCreate SaveToken(TokenModelCreate token);
        TokenModelUpdate UpdateUserToken(TokenModelUpdate token, long userId);
    }
}
