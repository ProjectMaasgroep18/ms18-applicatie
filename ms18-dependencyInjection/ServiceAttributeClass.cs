
namespace Maasgroep.DependencyInjection
{
    internal class ServiceAttributeClass
    {
        public Lifetime? LifeTime { get; set; }
        public Type ServiceType { get; set; }
        public Type? ImplementationType { get; set; }
    }
}
