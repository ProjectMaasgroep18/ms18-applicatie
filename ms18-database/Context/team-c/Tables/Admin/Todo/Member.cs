
namespace Maasgroep.Database.ToDoList
{
	public record Member : TodoRecordActive
	{
		public long Id { get; set; }
		public string Name { get; set; }


		// EF ToDo properties
		public ICollection<ToDo> ToDoOwned { get; set; }
		public ICollection<ToDo> ToDoCreated { get; set; }
		public ICollection<ToDo> ToDoModified { get; set; }
		public ICollection<ToDo> ToDoDeleted { get; set; }

		// EF generic properties
		public ICollection<Member>? MembersCreated { get; set; }
		public ICollection<Member>? MembersModified { get; set; }
		public ICollection<Member>? MembersDeleted { get; set; }
	}
}
