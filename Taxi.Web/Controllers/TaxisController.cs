using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Web.Data;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Controllers
{
    public class TaxisController : Controller
    {
        private readonly DataContext _context;

        public TaxisController(DataContext context)
        {
            _context = context;
        }

        // GET: Taxis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Taxis.ToListAsync());
        }

        // GET: Taxis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaxiEntity taxiEntity = await _context.Taxis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiEntity == null)
            {
                return NotFound();
            }
            return View(taxiEntity);
        }

        // GET: Taxis/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Taxis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxiEntity taxiEntity)
        {
            if (ModelState.IsValid)
            {
                taxiEntity.Plaque = taxiEntity.Plaque.ToUpper();
                _context.Add(taxiEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taxiEntity);
        }

        // GET: Taxis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaxiEntity taxiEntity = await _context.Taxis.FindAsync(id);
            if (taxiEntity == null)
            {
                return NotFound();
            }
            return View(taxiEntity);
        }

        // POST: Taxis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,TaxiEntity taxiEntity)
        {
            if (id != taxiEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                taxiEntity.Plaque = taxiEntity.Plaque.ToUpper();
                _context.Update(taxiEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taxiEntity);
        }

        // GET: Taxis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaxiEntity taxiEntity = await _context.Taxis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiEntity == null)
            {
                return NotFound();
            }
            _context.Taxis.Remove(taxiEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        
    }
}
