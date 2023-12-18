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
                Permissions = GetPermissions(member.Id),
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

        /** Get all member permissions */
        public List<string> GetPermissions(long memberId)
        {
            var permissions = new Dictionary<string, bool>();
            var memberPermissions = Db.MemberPermission
                .Where(mp => mp.MemberId == memberId)
                .Join(Db.Permission, mp => mp.PermissionId, p => p.Id, (mp, p) => p.Name)
                .ToList();

            foreach (var permission in memberPermissions)
            {
                // Make sure members with e.g. "basePermission.subPermission" also have "basePermission" set
                var n = 1;
                var splitPermission = permission.Split(".");
                while (n <= splitPermission.Length)
                {
                    permissions.Add(String.Join('.', splitPermission.Take(n)), true);
                    n++;
                }
            }
            return permissions.Select(p => p.Key).ToList();
        }
    }
}
