namespace Maasgroep.Exceptions;
public class MaasgroepBadRequest : MaasgroepException
{
    public MaasgroepBadRequest(string? message = null) : base(400, message ?? "Ongeldige gegevens") {}
}