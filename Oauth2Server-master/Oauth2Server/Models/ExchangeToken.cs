using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oauth2Server.Models
{
    public class ExchangeToken
    {
        public string ExchangeCode { get; set; }
        public Credential Credential { get; set; }
    }
}
