using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MagazinElectronice_Pascu_Ioana.Models;
using Microsoft.EntityFrameworkCore;
using MagazinElectronice_Pascu_Ioana.Data;
using MagazinElectronice_Pascu_Ioana.Models.MagazinViewModels;

namespace MagazinElectronice_Pascu_Ioana.Controllers
{
    public class HomeController : Controller
    {
        private readonly MagazinElectronice _context;
        public HomeController(MagazinElectronice context)
        {
            _context = context;
        }

        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<GrupComanda> data =
            from comanda in _context.Comenzi
            group comanda by comanda.DataComanda into dateGroup
            select new GrupComanda()
            {
                DataComanda = dateGroup.Key,
                DeviceCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}
