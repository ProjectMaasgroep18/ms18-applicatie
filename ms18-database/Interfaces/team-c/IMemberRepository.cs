
namespace Maasgroep.Database.Members
{
    public interface IMemberRepository
    {
        void AddMember(); // Member kan eventueel nog weg, gewoon Add()
        void RemoveMember();

        void AanmakenTestData(); // Even voor nu. Het nu van repo direct zichtbaar in consoleapp.

    }
}
