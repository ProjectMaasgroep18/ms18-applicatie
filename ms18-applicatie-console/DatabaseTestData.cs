using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Console
{
    internal class DatabaseTestData
    {
        private readonly MemberRepository _members;
        private readonly ReceiptRepository _receipts;

        internal DatabaseTestData() 
        {
           _members = new MemberRepository();
           _receipts = new ReceiptRepository();
        }

        internal void CreateTestDataAll()
        {
            // Members Repo
            _members.AanmakenTestData();

            // Receipts Repo
            _receipts.AanmakenTestData();
        }
    }
}
