
namespace Maasgroep.Database.ToDoList
{
    public record ToDoHistory : GenericRecordHistory
    {
        public long ToDoId { get; set; }
        public string Action { get; set; }
        public DateTime? Deadline { get; set; }
        public long MemberId { get; set; }
        public bool Done { get; set; }
    }
}
