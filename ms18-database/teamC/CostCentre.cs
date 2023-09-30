﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record CostCentre
	{
		[Key]
		public long Id { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string Name { get; set; } //Unique constraint in Builder.


		//Generic
		public long UserCreatedId { get; set; }
		public long? UserModifiedId { get; set; }
		public DateTime DateTimeCreated { get; set; }
		public DateTime? DateTimeModified { get; set; }


		//Ef
		public ICollection<Receipt> Receipt { get; set; }
		public MaasgroepMember UserCreated { get; set; }
		public MaasgroepMember? UserModified { get; set; }
	}
}
