
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maasgroep.SharedKernel.ViewModels.team_c.Authentication
{
    public class TokenModel
    {
        public TokenModel() { }

        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
