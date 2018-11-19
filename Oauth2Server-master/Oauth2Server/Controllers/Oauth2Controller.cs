using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Oauth2Server.Models;
using PasswordHandle;

namespace Oauth2Server.Controllers
{
    public class Oauth2Controller : Controller
    {
        private readonly Oauth2ServerContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly string OAUTH2_COOKIE = "oauth2token";


        public Oauth2Controller(Oauth2ServerContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult CheckToken(string tokenKey, string scopes)
        {
            //return Json(new {tokenKey, scopes});
            var token = _context.Credential.SingleOrDefault(t => t.AccessToken == tokenKey);
            if (token != null && token.IsValid() && token.IsValid(scopes))
            {
                return Ok(token);
            }
            return StatusCode(403);
        }

        [HttpGet]
        public IActionResult Authentication(string redirectUrl)
        {
            if (Request.Cookies.ContainsKey(OAUTH2_COOKIE))
            {
                var tokenString = Request.Cookies[OAUTH2_COOKIE];
                var token = _context.Credential.Find(tokenString);
                if (token != null && token.IsValid())
                {
                    return Redirect(redirectUrl);
                }
            }
            LoginInformation loginInformation = new LoginInformation
            {
                RedirectUrl = redirectUrl
            };
            return View("Login", loginInformation);
        }

        [HttpPost]
        [EnableCors("MyPolicy")]

        public IActionResult Authentication(LoginInformation loginInformation)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginInformation);
            }

            Account existAccount = _context.Account.FirstOrDefault(m => m.Email == loginInformation.Email);
            if (existAccount == null)
            {
                return View("Login", loginInformation);
            }

            if (PasswordHandle.PasswordHandle.GetInstance().EncryptPassword(loginInformation.Password, existAccount.Salt) != existAccount.Password)
            {
                return View("Login", loginInformation);
            }

            Credential credential = Credential.GenerateCredential(existAccount.Id, new List<CredentialScope>() {
                CredentialScope.Basic
            });
            _context.Credential.Add(credential);
            _context.SaveChanges();
            Response.Cookies.Append(
                OAUTH2_COOKIE,
                credential.AccessToken,
                new CookieOptions()
                {
                    Path = "/"
                }
            );
            return Redirect(loginInformation.RedirectUrl);
        }

        [HttpGet]
        public IActionResult Authorization(string clientId, string scopes)
        {
            if (Request.Cookies.ContainsKey(OAUTH2_COOKIE))
            {
                var tokenString = Request.Cookies[OAUTH2_COOKIE];
                var token = _context.Credential.Find(tokenString);
                if (token != null && token.IsValid())
                {
                    var client = _context.RegisteredClient.Find(clientId);
                    if (client == null)
                    {
                        return NotFound("Invalid client.");
                    }

                    Dictionary<string, Oauth2Scope> scopeItems = AvailableScopes.GetOauth2Scopes(scopes);
                    if (scopeItems.Count == 0)
                    {
                        return NotFound("Invalid scopes.");
                    }

                    var authorizationInformation = new AuthorizationInformation
                    {
                        RegisteredClient = client,
                        Oauth2Scopes = scopeItems
                    };
                    return View(authorizationInformation);
                }
            }
            return RedirectToAction("Authentication", new { redirectUrl = Request.GetDisplayUrl() });
        }

        [HttpPost]
        [EnableCors("MyPolicy")]

        public IActionResult GetAuthorizationExchangeCode(string clientId, string scopes)
        {
            if (Request.Cookies.ContainsKey(OAUTH2_COOKIE))
            {
                var tokenString = Request.Cookies[OAUTH2_COOKIE];
                var token = _context.Credential.Find(tokenString);
                if (token != null && token.IsValid())
                {
                    var client = _context.RegisteredClient.Find(clientId);
                    if (client == null)
                    {
                        return NotFound();
                    }

                    var exchange = new ExchangeToken()
                    {
                        ExchangeCode = Guid.NewGuid().ToString(),
                        Credential = Credential.GenerateCredential(token.AccountId, scopes)
                    };
                    _memoryCache.Set(exchange.ExchangeCode, exchange,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(180)));
                    return Redirect(client.RedirectUrl + "?code=" + exchange.ExchangeCode);
                }
            }
            return RedirectToAction("Authentication", new { redirectUrl = Request.GetDisplayUrl() });
        }

        [EnableCors("MyPolicy")]
        [HttpGet]
        public IActionResult ExchangeToken(string exchangeCode)
        {
            if (_memoryCache.TryGetValue(exchangeCode, out ExchangeToken exchangeToken))
            {
                _context.Credential.Add(exchangeToken.Credential);
                _context.SaveChanges();
                _memoryCache.Remove(exchangeCode);
                return Json(exchangeToken.Credential);
            }
            return NotFound();
        }
    }
}