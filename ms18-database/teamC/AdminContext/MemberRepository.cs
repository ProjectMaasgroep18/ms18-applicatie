
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

        public void AanmakenTestData()
        {
            CreateTestDataMember();
            CreateTestDataPermissions();
            CreateTestDataMemberPermissions();
        }

        private void CreateTestDataMember()
        {
            using (_db)
            {
                var members = new List<Member>()
                {
                    new Member() { Name = "Borgia"}
                };

                _db.Member.AddRange(members);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");

                var borgia = _db.Member.FirstOrDefault()!;

                members = new List<Member>()
                {
                    new Member() { Name = "da Gama", UserCreated = borgia }
                ,   new Member() { Name = "Albuquerque", UserCreated = borgia }
                };

                borgia.UserCreated = borgia;
                borgia.UserModified = borgia;
                borgia.DateTimeModified = DateTime.UtcNow;

                _db.Member.AddRange(members);

                rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataPermissions()
        {
            using (_db)
            {
                var borgia = _db.Member.FirstOrDefault()!;

                var permissions = new List<Permission>()
                {
                    new Permission() { Name = "receipt.approve", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.reject", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.handIn", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.payOut", UserCreated = borgia}
                };

                _db.Permission.AddRange(permissions);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataMemberPermissions()
        {
            using (_db)
            {
                var borgia = _db.Member.FirstOrDefault()!;
                var daGama = _db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
                var alb = _db.Member.Where(m => m.Name == "Albuquerque").FirstOrDefault()!;

                var approve = _db.Permission.Where(p => p.Name == "receipt.approve").FirstOrDefault()!;
                var reject = _db.Permission.Where(p => p.Name == "receipt.reject").FirstOrDefault()!;
                var handIn = _db.Permission.Where(p => p.Name == "receipt.handIn").FirstOrDefault()!;

                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { Member = daGama, Permission = approve, UserCreated = borgia }
                ,   new MemberPermission() { Member = daGama, Permission = reject, UserCreated = borgia }
                ,   new MemberPermission() { Member = alb, Permission = handIn, UserCreated = borgia }
                };

                _db.MemberPermission.AddRange(memberPermissions);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

    }
}
