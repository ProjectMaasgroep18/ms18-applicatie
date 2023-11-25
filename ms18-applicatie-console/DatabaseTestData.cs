using Maasgroep.Database.Members;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Console
{
    internal class DatabaseTestData
    {
        private readonly MemberRepository _members;
        private readonly ReceiptRepository _receipts;
        private readonly OrderRepository _orders;

        internal DatabaseTestData() 
        {
            _members = new MemberRepository();
            _receipts = new ReceiptRepository();
            _orders = new OrderRepository();
        }

        internal void CreateTestDataAll()
        {
            // Members Repo
            _members.AanmakenTestData();

            // Receipts Repo
            _receipts.AanmakenTestData();

            // Orders Repo
            _orders.AanmakenTestData();
        }
    }
}
