using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oauth2Server.Models
{
    public class RegisteredClient
    {
        [Key]
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDomain { get; set; }
        public string ClientIcon { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ClientStatus Status { get; set; }

        public RegisteredClient()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = ClientStatus.Activated;
        }
    }

    public enum ClientStatus
    {
        Activated = 1,
        Deactivated = 0
    }
}

