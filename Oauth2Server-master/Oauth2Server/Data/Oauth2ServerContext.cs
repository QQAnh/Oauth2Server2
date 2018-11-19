using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oauth2Server.Models
{
    public class Oauth2ServerContext : DbContext
    {
        public Oauth2ServerContext (DbContextOptions<Oauth2ServerContext> options)
            : base(options)
        {
        }

        public DbSet<Oauth2Server.Models.Account> Account { get; set; }
        public DbSet<Oauth2Server.Models.Credential> Credential { get; set; }

        public DbSet<Oauth2Server.Models.RegisteredClient> RegisteredClient { get; set; }
    }
}
