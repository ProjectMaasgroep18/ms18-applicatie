namespace Maasgroep.Exceptions;
public class MaasgroepForbidden : MaasgroepException
{
    public MaasgroepForbidden(string? message = null) : base(403, message ?? "Je hebt geen toegang tot dit onderdeel") {}
}