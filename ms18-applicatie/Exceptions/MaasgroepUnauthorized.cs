namespace Maasgroep.Exceptions;
public class MaasgroepUnauthorized : MaasgroepException
{
    public MaasgroepUnauthorized(string? message = null) : base(401, message ?? "Je bent niet ingelogd") {}
}