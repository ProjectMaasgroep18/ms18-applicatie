using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Test.ConsoleApp
{
    internal class DatabaseTestData
    {
        private readonly IMemberRepository _members;
        private readonly IReceiptRepository _receipts;

        internal DatabaseTestData(IReceiptRepository receipts, IMemberRepository members)
        {
            _members = members;
            _receipts = receipts;
        }

        public string doemijString()
        {
            return _receipts.GetStringDI();
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
