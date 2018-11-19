using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oauth2Server.Models
{
    public class Account
    {
        public Account()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Status = AccountStatus.Activated;
        }

        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AccountStatus Status { get; set; }
    }

    public enum AccountStatus
    {
        Activated = 1,
        Deactivated = 0
    }
}

