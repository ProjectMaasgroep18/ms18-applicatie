using Maasgroep.Database.Members;

namespace Maasgroep.Database.Context.ToDoList
{
    public record ToDo : GenericRecord
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public DateTime Deadline { get; set; }
        public long MemberId { get; set; }
        public bool Done { get; set; }

        // Ef
        public Member? Member { get; set; }
    }
}
