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
    [Authorize(Roles = "Angajat")]
    public class OferteController : Controller
    {
        private readonly MagazinElectronice _context;

        public OferteController(MagazinElectronice context)
        {
            _context = context;
        }

        // GET: Oferte
        [AllowAnonymous]
        public async Task<IActionResult> Index(string ordineSortare, string currentFilter, string stringCautare, int? numarPagina)
        {
            ViewData["CurrentSort"] = ordineSortare;
            ViewData["DenumireSortParm"] = String.IsNullOrEmpty(ordineSortare) ? "denumire_desc" : "";
            ViewData["PretRedusSortParm"] = ordineSortare == "PretRedus" ? "pretredus_desc" : "PretRedus";
            ViewData["CurrentFilter"] = stringCautare;

            if (stringCautare != null)
            {
                numarPagina = 1;
            }
            else
            {
                stringCautare = currentFilter;
            }


            var oferte = from o in _context.Oferte
                          select o;

            if (!String.IsNullOrEmpty(stringCautare))
            {
                oferte = oferte.Where(s => s.Denumire.Contains(stringCautare));
            }

            switch (ordineSortare)
            {
                case "denumire_desc":
                    oferte = oferte.OrderByDescending(o => o.Denumire);
                    break;
                case "PretRedus":
                    oferte = oferte.OrderBy(o => o.PretRedus);
                    break;
                case "pretredus_desc":
                    oferte = oferte.OrderByDescending(o => o.PretRedus);
                    break;
                default:
                    oferte = oferte.OrderBy(o => o.Denumire);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Oferta>.CreateAsync(oferte.AsNoTracking(), numarPagina ?? 1, pageSize));
        }

        // GET: Oferte/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferte
                .Include(s => s.Comenzi)
                .ThenInclude(e => e.Membru)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Oferte/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Oferte/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Denumire,PretVechi,PretRedus,Valabilitate")] Oferta oferta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(oferta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Nu se pot salva modificarile. " + "Incercati din nou si daca problema persista ");
            }

            return View(oferta);
        }

        // GET: Oferte/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferte.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }

        // POST: Oferte/Edit/5
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

            var ofertaToUpdate = await _context.Oferte.FirstOrDefaultAsync(o => o.ID == id);

            if (await TryUpdateModelAsync<Oferta>(
                ofertaToUpdate,
                "",
                o => o.Denumire, o => o.PretVechi, o => o.PretRedus, o => o.Valabilitate))
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
            return View(ofertaToUpdate);
        }

        // GET: Oferte/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferte
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (oferta == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Stergere eronata. Incercati din nou";
            }

            return View(oferta);
        }

        // POST: Oferte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Oferte.FindAsync(id);
            if (oferta == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Oferte.Remove(oferta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool OfertaExists(int id)
        {
            return _context.Oferte.Any(e => e.ID == id);
        }
    }
}
