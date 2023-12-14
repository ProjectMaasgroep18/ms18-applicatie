using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Admin
{
    public class MemberRepository : DeletableRepository<Member, MemberModel>, IMemberRepository
    {
        public MemberRepository(MaasgroepContext db) : base(db) {}

		/** Create MemberModel from Member record */
        public override MemberModel GetModel(Member member)
        {
            return new MemberModel() {
				Name = member.Name,
			};
        }

		/** Create or update Member record from model */
        public override Member? GetRecord(MemberModel model, Member? existingMember = null)
        {
            var member = existingMember ?? new();
			member.Name = model.Name;
			return member;
        }
    }
}
