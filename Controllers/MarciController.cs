using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazinElectronice_Pascu_Ioana.Data;
using MagazinElectronice_Pascu_Ioana.Models;
using MagazinElectronice_Pascu_Ioana.Models.MagazinViewModels;
using Microsoft.AspNetCore.Authorization;


namespace MagazinElectronice_Pascu_Ioana.Controllers
{
    [Authorize(Policy = "OnlyAngajat")]
    public class MarciController : Controller
    {
        private readonly MagazinElectronice _context;

        public MarciController(MagazinElectronice context)
        {
            _context = context;
        }

        // GET: Marci
        public async Task<IActionResult> Index(int? id, int? deviceID)
        {
            var viewModel = new MarcaIndexData();
            viewModel.Marci = await _context.Marci
                .Include(i => i.MarcaDevices)
                 .ThenInclude(i => i.Device)
                    .ThenInclude(i => i.Comenzi)
                        .ThenInclude(i => i.Client)
                 .AsNoTracking()
                 .OrderBy(i => i.DenumireMarca)
                 .ToListAsync();

            if (id != null)
            {
                ViewData["MarcaID"] = id.Value;
                Marca marca = viewModel.Marci.Where(
                i => i.ID == id.Value).Single();
                viewModel.Devices = marca.MarcaDevices.Select(s => s.Device);
            }
            if (deviceID != null)
            {
                ViewData["DeviceID"] = deviceID.Value;
                viewModel.Comenzi = viewModel.Devices.Where(
                x => x.ID == deviceID).Single().Comenzi;
            }
            return View(viewModel);
        }

        // GET: Marci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marci
                .FirstOrDefaultAsync(m => m.ID == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DenumireMarca,Fondator")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        // GET: Marci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marci
                 .Include(i => i.MarcaDevices).ThenInclude(i => i.Device)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.ID == id);

            if (marca == null)
            {
                return NotFound();
            }
            PopulateMarcaDeviceData(marca);
            return View(marca);
        }

        private void PopulateMarcaDeviceData(Marca marca)
        {
            var allDevices = _context.Devices;
            var marcaDevices = new HashSet<int>(marca.MarcaDevices.Select(m => m.DeviceID));
            var viewModel = new List<MarcaDeviceData>();
            foreach(var device in allDevices)
            {
                viewModel.Add(new MarcaDeviceData
                {
                    DeviceID = device.ID,
                    Denumire = device.Denumire,
                    IsMarca = marcaDevices.Contains(device.ID)
                });
            }
            ViewData["Devices"] = viewModel;
        }

        // POST: Marci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedDevices) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var marcaToUpdate = await _context.Marci
                 .Include(i => i.MarcaDevices)
                 .ThenInclude(i => i.Device)
                 .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Marca>(
            marcaToUpdate,
            "",
            i => i.DenumireMarca, i => i.Fondator))
            {
                UpdateMarcaDevices(selectedDevices, marcaToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Nu se pot salva modificarile. " + "Incercati din nou daca problema persista ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateMarcaDevices(selectedDevices, marcaToUpdate);
            PopulateMarcaDeviceData(marcaToUpdate);
            return View(marcaToUpdate);
        }
        private void UpdateMarcaDevices(string[] selectedDevices, Marca marcaToUpdate)
        {
            if (selectedDevices == null)
            {
                marcaToUpdate.MarcaDevices = new List<MarcaDevice>();
                return;
            }
            var selectedDevicesHS = new HashSet<string>(selectedDevices);
            var marcaDevices = new HashSet<int>
            (marcaToUpdate.MarcaDevices.Select(c => c.Device.ID));
            foreach (var device in _context.Devices)
            {
                if (selectedDevicesHS.Contains(device.ID.ToString()))
                {
                    if (!marcaDevices.Contains(device.ID))
                    {
                        marcaToUpdate.MarcaDevices.Add(new MarcaDevice
                        {
                            MarcaID = marcaToUpdate.ID,
                            DeviceID = device.ID
                        });
                    }
                }
                else
                {
                    if (marcaDevices.Contains(device.ID))
                    {
                        MarcaDevice deviceToRemove = marcaToUpdate.MarcaDevices.FirstOrDefault(i
                       => i.DeviceID == device.ID);
                        _context.Remove(deviceToRemove);
                    }
                }
            }
        }

        // GET: Marci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marci
                .FirstOrDefaultAsync(m => m.ID == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await _context.Marci.FindAsync(id);
            _context.Marci.Remove(marca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
            return _context.Marci.Any(e => e.ID == id);
        }
    }
}
