using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maasgroep.SharedKernel.Interfaces.Token;
using Maasgroep.SharedKernel.ViewModels.team_c.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Tokens
{
    public class TokenStoreRepository : ITokenStoreRepository
    {
        private readonly MaasgroepContext _db;
        public TokenStoreRepository(MaasgroepContext db)
        {
            _db = db;
        }

        public TokenModel RetrieveToken(long memberId)
        {
            var savedToken = _db.TokenStore.FirstOrDefault(x => x.MemberId == memberId);

            if (savedToken == null)
            {
                return null;
            }
            else
            {
                TokenModel token = new TokenModel
                {
                    Token = savedToken.Token,
                    ExpirationDate = savedToken.ExperationDate,
                };

                return token;
            }
        }

        public TokenModelCreate SaveToken(TokenModelCreate token)
        {
            var member = _db.Member.FirstOrDefault(m => m.Id == token.MemberId);
            if (member == null) throw new Exception("Member niet gevonden");

            var newToken = new TokenStore()
            {
                Token = token.Token,
                ExperationDate = token.ExpirationDate,
                MemberId = member.Id,
            };

            _db.TokenStore.Add(newToken);
            _db.SaveChanges();

            return token;
        }

        public TokenModelUpdate UpdateUserToken(TokenModelUpdate token, long userId)
        {
            var savedToken = _db.TokenStore.FirstOrDefault(m => m.MemberId == userId);
            if (savedToken == null) throw new Exception("Token niet gevonden");

            savedToken.Token = token.Token;
            savedToken.ExperationDate = token.ExpirationDate;
            savedToken.MemberId = userId;

            _db.TokenStore.Update(savedToken);
            _db.SaveChanges();

            return token;
        }
    }
}
