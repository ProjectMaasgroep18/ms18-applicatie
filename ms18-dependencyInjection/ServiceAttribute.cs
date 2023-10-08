
namespace Maasgroep.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {

        }

        public ServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
            Lifetime = Lifetime.Scoped;
        }

        public Lifetime Lifetime { get; set; }
        public Type ServiceType { get; set; }
    }
}

