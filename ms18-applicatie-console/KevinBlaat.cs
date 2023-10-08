using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Microsoft.Extensions.Hosting;

namespace Maasgroep.Test.ConsoleApp
{
    public class BackGroundConsole : BackgroundService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IHostApplicationLifetime _applicationLifetime;
        public BackGroundConsole(IReceiptRepository r, IMemberRepository m, IHostApplicationLifetime alles)
        {
            _receiptRepository = r;
            _memberRepository = m;
            _applicationLifetime = alles;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var isDone = false;

            var d = new DatabaseTestData(_receiptRepository, _memberRepository);

            Console.WriteLine($"Dikke Dependenciy injection: {d.doemijString()}");

            while (!stoppingToken.IsCancellationRequested && !isDone)
            {
                Console.WriteLine("Dit komt ertwel?");
                Console.WriteLine("Type een zin en je krijgt hem terug. Type 'q' voor doei.");
                var hoi = Console.ReadLine();
                Console.WriteLine($"You type {hoi}");

                if (hoi == "q")
                {
                    isDone = true;
                    _applicationLifetime.StopApplication();
                }
                //await Task.Delay(1000);
            }
        }
    }
}
