using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazinElectronice_Pascu_Ioana.Data;
using MagazinElectronice_Pascu_Ioana.Models;

namespace MagazinElectronice_Pascu_Ioana.Controllers
{
    public class DevicesController : Controller
    {
        private readonly MagazinElectronice _context;

        public DevicesController(MagazinElectronice context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index(string ordineSortare, string currentFilter, string stringCautare, int? numarPagina)
        {
            ViewData["CurrentSort"] = ordineSortare;
            ViewData["DenumireSortParm"] = String.IsNullOrEmpty(ordineSortare) ? "denumire_desc" : "";
            ViewData["PretSortParm"] = ordineSortare == "Pret" ? "pret_desc" : "Pret";
            ViewData["CurrentFilter"] = stringCautare;

            if(stringCautare != null)
            {
                numarPagina = 1;
            }
            else
            {
                stringCautare = currentFilter;
            }


            var devices = from d in _context.Devices
                          select d;
            if (!String.IsNullOrEmpty(stringCautare))
            {
                devices = devices.Where(s => s.Denumire.Contains(stringCautare));
            }
            switch (ordineSortare)
            {
                case "denumire_desc":
                    devices = devices.OrderByDescending(d => d.Denumire);
                    break;
                case "Pret":
                    devices = devices.OrderBy(d => d.Pret);
                    break;
                case "pret_desc":
                    devices = devices.OrderByDescending(d => d.Pret);
                    break;
                default:
                    devices = devices.OrderBy(d => d.Denumire);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Device>.CreateAsync(devices.AsNoTracking(), numarPagina ??1, pageSize));
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(s => s.Comenzi)
                .ThenInclude(e => e.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Denumire,Descriere,Pret")] Device device)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(device);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Nu se pot salva modificarile. " + "Incercati din nou si daca problema persista ");
            }
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
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

            var clientToUpdate = await _context.Devices.FirstOrDefaultAsync(c => c.ID == id);

            if (await TryUpdateModelAsync<Device>(
                clientToUpdate,
                "",
                c => c.Denumire, c => c.Descriere, c => c.Pret))
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

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (device == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] ="Stergere eronata. Incercati din nou";
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if(device == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
           
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.ID == id);
        }
    }
}
