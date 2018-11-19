using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oauth2Server.Models;

namespace Oauth2Server.Controllers
{
    public class RegisteredClientsController : Controller
    {
        private readonly Oauth2ServerContext _context;

        public RegisteredClientsController(Oauth2ServerContext context)
        {
            _context = context;
        }

        // GET: RegisteredClients
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegisteredClient.ToListAsync());
        }

        // GET: RegisteredClients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredClient = await _context.RegisteredClient
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (registeredClient == null)
            {
                return NotFound();
            }

            return View(registeredClient);
        }

        // GET: RegisteredClients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegisteredClients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,ClientName,ClientDomain,ClientIcon,RedirectUrl,CreatedAt,UpdatedAt,Status")] RegisteredClient registeredClient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registeredClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registeredClient);
        }

        // GET: RegisteredClients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredClient = await _context.RegisteredClient.FindAsync(id);
            if (registeredClient == null)
            {
                return NotFound();
            }
            return View(registeredClient);
        }

        // POST: RegisteredClients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ClientId,ClientName,ClientDomain,ClientIcon,RedirectUrl,CreatedAt,UpdatedAt,Status")] RegisteredClient registeredClient)
        {
            if (id != registeredClient.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registeredClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisteredClientExists(registeredClient.ClientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registeredClient);
        }

        // GET: RegisteredClients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredClient = await _context.RegisteredClient
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (registeredClient == null)
            {
                return NotFound();
            }

            return View(registeredClient);
        }

        // POST: RegisteredClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var registeredClient = await _context.RegisteredClient.FindAsync(id);
            _context.RegisteredClient.Remove(registeredClient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisteredClientExists(string id)
        {
            return _context.RegisteredClient.Any(e => e.ClientId == id);
        }
    }
}
