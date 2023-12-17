using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;

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
        public override bool Delete(long id, long memberId)
        {
            if (id == memberId)
                return false;
            return base.Delete(id, memberId);
        }
    }
}
