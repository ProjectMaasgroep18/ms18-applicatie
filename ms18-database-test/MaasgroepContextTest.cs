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
        public void CreateProduct()
        {
            using var context = Fixture.CreateContext();
            var controller = new StockController(context);

            //var stockie = controller.StockGetById(1) as ObjectResult;
            //var model = stockie.Value as StockViewModel;

            //Assert.Equal("Duifis Scharrelnootjes", model.Name);
            //Assert.Equal(5, model.Quantity);
        }
    }
}
