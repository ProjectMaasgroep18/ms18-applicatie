namespace Maasgroep.Exceptions;
public class MaasgroepNotFound : MaasgroepException
{
    public MaasgroepNotFound(string? message = null) : base(404, message ?? "Niet gevonden") {}
}