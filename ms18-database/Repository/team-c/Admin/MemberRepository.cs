using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;
using System.Security.Cryptography;
using System.Text;

namespace Maasgroep.Database.Admin
{
    public class MemberRepository : DeletableRepository<Member, MemberModel, MemberData>, IMemberRepository
    {
        public MemberRepository(MaasgroepContext db) : base(db) {}

		/** Create MemberModel from Member record */
        public override MemberModel GetModel(Member member)
        {
            return new MemberModel() {
                Id = member.Id,
				Name = member.Name,
                Email = member.Email,
			};
        }

		/** Create or update Member record from data model */
        public override Member? GetRecord(MemberData data, Member? existingMember = null)
        {
            var member = existingMember ?? new();
			member.Name = data.Name;
			return member;
        }

        /** Prevent deletion of oneself */
        public override bool Delete(Member member, long? memberId)
        {
            if (member.Id == memberId)
                return false;
            return base.Delete(member, memberId);
        }

        /** Get member by e-mail/password */
        public MemberModel? GetByEmail(string email, string password)
        {
            var member = Db.Member.FirstOrDefault(item => item.Email == email && item.DateTimeDeleted == null);
            if (member == null || !CheckPassword(password, member.Password))
                return null;
            return GetModel(member);
        }

        /** Get a hash for a password string */
        public string GetPasswordHash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        /** Check if password is correct for hash */
        public bool CheckPassword(string password, string hash)
            => hash == GetPasswordHash(password);
    }
}
