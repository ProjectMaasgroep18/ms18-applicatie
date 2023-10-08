using Maasgroep.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Maasgroep.Test.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<BackGroundConsole>();
            var c = new MaasgroepServicesBuilder();
            var host = c.Iets(builder);



            await host.RunAsync();



            Environment.Exit(0);
        }
    }
}