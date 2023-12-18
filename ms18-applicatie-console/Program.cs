
namespace Maasgroep.Console
{
    internal class Program
    {
        static void Main()
        {
            System.Console.WriteLine("Populating database");

            var database = new DatabaseTestData();
            database.CreateTestData();
        }
    }
}