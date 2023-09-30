namespace ms18_applicatie_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var database = new DatabaseTestData();
            database.CreateTestDataAll();
        }
    }
}