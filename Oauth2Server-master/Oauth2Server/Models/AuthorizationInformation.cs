using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oauth2Server.Models
{
    public class AuthorizationInformation
    {
        public RegisteredClient RegisteredClient { get; set; }
        public Dictionary<string, Oauth2Scope> Oauth2Scopes { get; set; }
    }
}
