
namespace Maasgroep.Database.Members
{
    public class MemberRepository : IMemberRepository
    {
        public MemberRepository()
        { 
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
                    new Member() { Name = "da Gama", UserCreated = borgia }
                ,   new Member() { Name = "Albuquerque", UserCreated = borgia }
                };

                borgia.UserCreated = borgia;
                borgia.UserModified = borgia;
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
                    new Permission() { Name = "receipt.approve", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.reject", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.handIn", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.payOut", UserCreated = borgia}
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
                    new MemberPermission() { Member = daGama, Permission = approve, UserCreated = borgia }
                ,   new MemberPermission() { Member = daGama, Permission = reject, UserCreated = borgia }
                ,   new MemberPermission() { Member = alb, Permission = handIn, UserCreated = borgia }
                };

                db.MemberPermission.AddRange(memberPermissions);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

    }
}
