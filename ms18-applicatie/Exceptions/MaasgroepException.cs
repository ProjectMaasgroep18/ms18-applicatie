namespace Maasgroep.Exceptions;
public class MaasgroepException : Exception
{
    public MaasgroepException(int statusCode, string message) =>
        (StatusCode, Message) = (statusCode, message);

    public int StatusCode { get; }
    public new string Message { get; }
}