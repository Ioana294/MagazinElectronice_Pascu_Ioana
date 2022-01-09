using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazinElectronice_Pascu_Ioana.Data;
using MagazinElectronice_Pascu_Ioana.Models;
using Microsoft.AspNetCore.Authorization;

namespace MagazinElectronice_Pascu_Ioana.Controllers
{
    [Authorize(Policy = "RolManager")]
    public class ClientsController : Controller
    {
        private readonly MagazinElectronice _context;

        public ClientsController(MagazinElectronice context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string ordineSortare, string currentFilter, string stringCautare, int? numarPagina)
        {
            ViewData["CurrentSort"] = ordineSortare;
            ViewData["NumeSortParm"] = String.IsNullOrEmpty(ordineSortare) ? "nume_desc" : "";
            ViewData["TotalSortParm"] = ordineSortare == "Total" ? "total_desc" : "Total";
            ViewData["CurrentFilter"] = stringCautare;

            if (stringCautare != null)
            {
                numarPagina = 1;
            }
            else
            {
                stringCautare = currentFilter;
            }

            var clients = from c in _context.Clienti
                        select c;

            if (!String.IsNullOrEmpty(stringCautare))
            {
                clients = clients.Where(s => s.Nume.Contains(stringCautare));
            }
            switch (ordineSortare)
            {
                case "nume_desc":
                    clients = clients.OrderByDescending(c => c.Nume);
                    break;
                case "Total":
                    clients = clients.OrderBy(c => c.Total);
                    break;
                case "total_desc":
                    clients = clients.OrderByDescending(c => c.Total);
                    break;
                default:
                    clients = clients.OrderBy(c => c.Nume);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), numarPagina ?? 1, pageSize));
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienti
                .Include(s => s.Comenzi)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nume,Prenume,Adresa,Total")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }    
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Nu se pot salva modificarile. " + "Incercati din nou si daca problema persista ");

            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienti.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientToUpdate = await _context.Clienti.FirstOrDefaultAsync(c => c.ID == id);

            if (await TryUpdateModelAsync<Client>(
                clientToUpdate,
                "",
                c => c.Nume, c => c.Prenume, c => c.Adresa, c => c.Total))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Nu se pot salva modificarile." + "Incercati din nou si daca problema persista");
                }
            }
            return View(clientToUpdate);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienti
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Stergere eronata. Incercati din nou";
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clienti.FindAsync(id);
            if(client == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Clienti.Remove(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ClientExists(int id)
        {
            return _context.Clienti.Any(e => e.ID == id);
        }
    }
}
