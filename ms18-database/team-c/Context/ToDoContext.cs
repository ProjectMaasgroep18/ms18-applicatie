using Maasgroep.Database.ToDoList;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database
{
	public class ToDoContext : DbContext
	{
		private readonly string _connectionString;

		public DbSet<Member> Member { get; set; }
		public DbSet<ToDo> ToDos { get; set; }
		public DbSet<ToDoHistory> ToDoHistory { get; set; }


		public ToDoContext()
		{
			_connectionString = "UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true";
		}

		public ToDoContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			CreateToDo(modelBuilder);
			CreateToDoHistory(modelBuilder);
		}

		#region Todo

		private void CreateToDo(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ToDo>().ToTable("todo", "todo");
			modelBuilder.Entity<ToDo>().HasKey(t => t.Id);
			modelBuilder.HasSequence<long>("todoSeq", "todo").StartsAt(1).IncrementsBy(1); ;
			modelBuilder.Entity<ToDo>().Property(t => t.Id).HasDefaultValueSql("nextval('todo.\"todoSeq\"')");
			modelBuilder.Entity<ToDo>().Property(t => t.Action).HasMaxLength(2048);

			modelBuilder.Entity<ToDo>()
				.HasOne(t => t.Member)
				.WithMany(m => m.ToDoOwned)
				.HasForeignKey(t => t.MemberId)
				.HasConstraintName("FK_todo_memberOwned")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ToDo>()
				.HasOne(ra => ra.MemberCreated)
				.WithMany(m => m.ToDoCreated)
				.HasForeignKey(ra => ra.MemberCreatedId)
				.HasConstraintName("FK_todo_memberCreated")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ToDo>()
				.HasOne(ra => ra.MemberModified)
				.WithMany(m => m.ToDoModified)
				.HasForeignKey(ra => ra.MemberModifiedId)
				.HasConstraintName("FK_todo_memberModified")
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<ToDo>()
				.HasOne(ra => ra.MemberDeleted)
				.WithMany(m => m.ToDoDeleted)
				.HasForeignKey(ra => ra.MemberDeletedId)
				.HasConstraintName("FK_todo_memberDeleted")
				.OnDelete(DeleteBehavior.NoAction);
		}

		#endregion

		#region TodoHistory

		private void CreateToDoHistory(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ToDoHistory>().ToTable("todo", "todoHistory");
			modelBuilder.Entity<ToDoHistory>().HasKey(t => t.Id);
			modelBuilder.HasSequence<long>("todoSeq", "todoHistory").StartsAt(1).IncrementsBy(1); ;
			modelBuilder.Entity<ToDoHistory>().Property(t => t.Id).HasDefaultValueSql("nextval('\"todoHistory\".\"todoSeq\"')");
			modelBuilder.Entity<ToDoHistory>().Property(t => t.Action).HasMaxLength(2048);
		}

		#endregion
	}
}
