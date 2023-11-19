using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Controllers.Api.team_c;

namespace Maasgroep.Database.Test
{
    // Documentatie
    // https://learn.microsoft.com/en-us/ef/core/testing/
    // https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0

    public class MaasgroepContextTest : IClassFixture<MaasgroepTestFixture>
    {
        public MaasgroepContextTest(MaasgroepTestFixture fixture)
        => Fixture = fixture;

        public MaasgroepTestFixture Fixture { get; }

        [Fact]
        public void GetProduct()
        {
            using var context = Fixture.CreateContext();
            var controller = new StockController(context);

            var stockie = controller.StockGetById(1) as ObjectResult;
            var model = stockie.Value as StockViewModel;
            
            Assert.Equal("Duifis Scharrelnootjes", model.Name);
            Assert.Equal(5, model.Quantity);
        }

        [Fact]
        public void ModifyExistingStock()
        {
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();

            var controller = new StockController(context);

            var newStock = new StockViewModel("Duifis Scharrelnootjes", 1);

            controller.ModifyStock(newStock);

            context.ChangeTracker.Clear();

            var stockAdded = context.Stock.Single(b => b.ProductId == 1);
            var stockHistoryAdded = context.StockHistory.Where(b => b.ProductId == 1).OrderByDescending(x => x.Id).FirstOrDefault();
            Assert.Equal(1, stockAdded.ProductId);
            Assert.Equal(1, stockAdded.Quantity);
            Assert.Equal(1, stockHistoryAdded.ProductId);
            Assert.Equal(5, stockHistoryAdded.Quantity);
        }
    }
}
