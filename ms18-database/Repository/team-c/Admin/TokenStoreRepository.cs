using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Admin
{
    public class TokenStoreRepository : ReadOnlyRepository<TokenStore, TokenModel>, ITokenStoreRepository
    {
        public TokenStoreRepository(MaasgroepContext db) : base(db) {}

        public bool SaveToken(string token, DateTime expire, long memberId)
        {
            Db.Add(new TokenStore
            {
               Token = token,
               MemberId = memberId,
               ExperationDate = expire,
            });

            return Db.SaveChanges() > 0;
        }

        public long? GetMemberIdFromToken(string token)
            => Db.TokenStore.FirstOrDefault(t => t.Token == token && t.ExperationDate > DateTime.UtcNow)?.MemberId;

        public override TokenModel GetModel(TokenStore tokenRecord)
            => new()
            {
                Token = tokenRecord.Token,
                Member = new MemberRepository(Db).GetModel(tokenRecord.MemberId),
                ExpirationDate = tokenRecord.ExperationDate,
            };

        public bool DeleteToken(string token, long memberId)
        {
            var tokenRecord = Db.TokenStore.FirstOrDefault(t => t.Token == token && t.MemberId == memberId);
            if (tokenRecord == null)
                return false;
            Db.Remove(tokenRecord);
            return Db.SaveChanges() > 0;
        }

    }
}
