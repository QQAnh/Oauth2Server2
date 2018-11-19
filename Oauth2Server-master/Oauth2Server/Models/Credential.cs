using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oauth2Server.Models
{
    public class Credential
    {
        [Key]
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long AccountId { get; set; }
        public string Scope { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CredentialStatus Status { get; set; }

        public static Credential GenerateCredential(long accountId, List<CredentialScope> scopes)
        {
            var token = Guid.NewGuid().ToString();
            return new Credential()
            {
                AccessToken = token,
                RefreshToken = Guid.NewGuid().ToString(),
                AccountId = accountId,
                Scope = String.Join(",", scopes.ToArray()),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(7),
                Status = CredentialStatus.Activated
            };
        }

        public static Credential GenerateCredential(long accountId, string scopes)
        {
            var token = Guid.NewGuid().ToString();
            return new Credential()
            {
                AccessToken = token,
                RefreshToken = Guid.NewGuid().ToString(),
                AccountId = accountId,
                Scope = scopes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(7),
                Status = CredentialStatus.Activated
            };
        }

        internal bool IsValid(string scopes)
        {
            return true;
        }

        public bool IsValid()
        {
            return this.ExpiredAt > DateTime.Now;
        }
    }

    public enum CredentialStatus
    {
        Activated = 1,
        Deactivated = 0
    }

    public enum CredentialScope
    {
        Basic = 1,
        SongApi = 2,
        Gallery = 3,
        Contact = 4
    
}
}
