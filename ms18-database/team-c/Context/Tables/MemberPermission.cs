<<<<<<<< HEAD:ms18-applicatie/team-c/Models/MemberPermission.cs
﻿using ms18_applicatie.Models;

namespace ms18_applicatie.Models
========
﻿
namespace Maasgroep.Database.Members
>>>>>>>> feature/databaseOpzet:ms18-database/team-c/Context/Tables/MemberPermission.cs
{
    internal record MemberPermission
    {
        public long MemberId { get; set; }
        public long PermissionId { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF admin properties
        public Member Member { get; set; }
        public Permission Permission { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }

    }
}
