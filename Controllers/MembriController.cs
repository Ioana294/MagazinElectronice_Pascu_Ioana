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
    public class MembriController : Controller
    {
        private readonly MagazinElectronice _context;

        public MembriController(MagazinElectronice context)
        {
            _context = context;
        }

        // GET: Membri
        public async Task<IActionResult> Index(string ordineSortare, string currentFilter, string stringCautare, int? numarPagina)
        {
            ViewData["CurrentSort"] = ordineSortare;
            ViewData["PrenumeSortParm"] = String.IsNullOrEmpty(ordineSortare) ? "prenume_desc" : "";
            ViewData["PuncteSortParm"] = ordineSortare == "Puncte" ? "puncte_desc" : "Puncte";
            ViewData["CurrentFilter"] = stringCautare;

            if (stringCautare != null)
            {
                numarPagina = 1;
            }
            else
            {
                stringCautare = currentFilter;
            }


            var membri = from m in _context.Membri
                          select m;
            if (!String.IsNullOrEmpty(stringCautare))
            {
                membri = membri.Where(s => s.Nume.Contains(stringCautare));
            }
            switch (ordineSortare)
            {
                case "prenume_desc":
                    membri = membri.OrderByDescending(m => m.Prenume);
                    break;
                case "Puncte":
                    membri = membri.OrderBy(m => m.Puncte);
                    break;
                case "puncte_desc":
                    membri = membri.OrderByDescending(m => m.Puncte);
                    break;
                default:
                    membri = membri.OrderBy(m => m.Nume);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Membru>.CreateAsync(membri.AsNoTracking(), numarPagina ?? 1, pageSize));
        }

        // GET: Membri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membru = await _context.Membri
                .Include(s => s.Comenzi)
                .ThenInclude(E => E.Oferta)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (membru == null)
            {
                return NotFound();
            }

            return View(membru);
        }

        // GET: Membri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Membri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nume,Prenume,Adresa,Puncte")] Membru membru)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(membru);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
           catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Nu se pot salva modificarile. " + "Incercati din nou si daca problema persista ");
            }
            return View(membru);
        }

        // GET: Membri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membru = await _context.Membri.FindAsync(id);
            if (membru == null)
            {
                return NotFound();
            }
            return View(membru);
        }

        // POST: Membri/Edit/5
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

            var membruToUpdate = await _context.Membri.FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Membru>(
                membruToUpdate,
                "",
                m => m.Nume, m => m.Prenume, m => m.Adresa, m => m.Puncte))
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
            return View(membruToUpdate);
        }

        // GET: Membri/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membru = await _context.Membri
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (membru == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Stergere eronata. Incercati din nou";
            }

            return View(membru);
        }

        // POST: Membri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membru = await _context.Membri.FindAsync(id);
            if(membru == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Membri.Remove(membru);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool MembruExists(int id)
        {
            return _context.Membri.Any(e => e.ID == id);
        }
    }
}
