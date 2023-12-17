using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maasgroep.SharedKernel.ViewModels.team_c.Authentication
{
    public class TokenModelUpdate
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
