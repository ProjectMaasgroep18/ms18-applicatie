
namespace Maasgroep.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            var database = new DatabaseTestData();
            database.CreateTestDataAll();
        }
    }
}