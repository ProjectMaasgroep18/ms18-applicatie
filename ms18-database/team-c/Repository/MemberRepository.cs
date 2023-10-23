
using Maasgroep.Database.ViewModel;

namespace Maasgroep.Database.Members
{
    public class MemberRepository : IMemberRepository
    {
        public MemberRepository()
        { 
        }

        public MemberViewModel GetMember()
        {
            MemberViewModel result = null;

            using (var db = new MaasgroepContext())
            {
                var q=  db.Member.FirstOrDefault()!;

                result.Name = q.Name;

            }

            return result;
        }

        public void AddMember()
        {
            throw new NotImplementedException();
        }

        public void RemoveMember()
        {
            throw new NotImplementedException();
        }

        public void AanmakenTestData()
        {
            CreateTestDataMember();
            CreateTestDataPermissions();
            CreateTestDataMemberPermissions();
        }

        private void CreateTestDataMember()
        {
            using (var db = new MaasgroepContext())
            {
                var members = new List<Member>()
                {
                    new Member() { Name = "Borgia"}
                };

                db.Member.AddRange(members);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");

                var borgia = db.Member.FirstOrDefault()!;

                members = new List<Member>()
                {
                    new Member() { Name = "da Gama", MemberCreated = borgia }
                ,   new Member() { Name = "Albuquerque", MemberCreated = borgia }
                };

                borgia.MemberCreated = borgia;
                borgia.MemberModified = borgia;
                borgia.DateTimeModified = DateTime.UtcNow;

                db.Member.AddRange(members);

                rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataPermissions()
        {
            using (var db = new MaasgroepContext())
            {
                var borgia = db.Member.FirstOrDefault()!;

                var permissions = new List<Permission>()
                {
                    new Permission() { Name = "receipt.approve", MemberCreated = borgia}
                ,   new Permission() { Name = "receipt.reject", MemberCreated = borgia}
                ,   new Permission() { Name = "receipt.handIn", MemberCreated = borgia}
                ,   new Permission() { Name = "receipt.payOut", MemberCreated = borgia}
                };

                db.Permission.AddRange(permissions);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataMemberPermissions()
        {
            using (var db = new MaasgroepContext())
            {
                var borgia = db.Member.FirstOrDefault()!;
                var daGama = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
                var alb = db.Member.Where(m => m.Name == "Albuquerque").FirstOrDefault()!;

                var approve = db.Permission.Where(p => p.Name == "receipt.approve").FirstOrDefault()!;
                var reject = db.Permission.Where(p => p.Name == "receipt.reject").FirstOrDefault()!;
                var handIn = db.Permission.Where(p => p.Name == "receipt.handIn").FirstOrDefault()!;

                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { Member = daGama, Permission = approve, MemberCreated = borgia }
                ,   new MemberPermission() { Member = daGama, Permission = reject, MemberCreated = borgia }
                ,   new MemberPermission() { Member = alb, Permission = handIn, MemberCreated = borgia }
                };

                db.MemberPermission.AddRange(memberPermissions);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

    }
}
