using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Models
{
    public class CostCentre : ICostCentre
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
