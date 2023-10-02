
namespace Maasgroep.Database.Members
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberContext _db;
        public MemberRepository()
        { 
            _db = new MemberContext(); // Ombouwen
        }

        public void AddMember()
        {
            throw new NotImplementedException();
        }

        public void RemoveMember()
        {
            throw new NotImplementedException();
        }
    }
}
