using Maasgroep.Database;

namespace ms18_applicatie_console
{
    internal class DatabaseTestData
    {
        internal DatabaseTestData() { }

        internal void CreateTestDataAll()
        {
            CreateTestDataMember();
            CreateTestDataCostCentre();
        }

        internal void CreateTestDataMember()
        {

            using (var db = new MaasgroepContext())
            {
                var members = new List<MaasgroepMember>()
                {
                    new MaasgroepMember() { Name = "da Gama"}
                ,   new MaasgroepMember() { Name = "Borgia"}
                ,   new MaasgroepMember() { Name = "Albuquerque"}
                };

                db.MaasgroepMember.AddRange(members);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        internal void CreateTestDataCostCentre()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.MaasgroepMember.Where(m => m.Id == 1).FirstOrDefault()!;

                var costCentres = new List<CostCentre>()
                {
                    new CostCentre() { Name = "Albert Heijn", UserCreatedInstance = member}
                };

                db.CostCentre.AddRange(costCentres);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
    }
}
