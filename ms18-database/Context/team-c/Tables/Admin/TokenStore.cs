using Maasgroep.Database.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maasgroep.Database.Tokens
{
    public record TokenStore
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime ExperationDate { get; set; }
        
        //FK property
        public long? MemberId { get; set; }
        
        //EF property
        public Member? Member { get; set; }

    }
}
