using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyApp.Data;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Controllers
{
    public class ItemsController : Controller
    {
        public MyAppContext _context;

        public ItemsController(MyAppContext context)
        {
            _context  = context; 
        }

        public IActionResult Overview()
        {   
            var item = new Item() {Name = "keyboard"};
            return View(item);
        }

        public async Task<IActionResult> Index()
        {
            var item = await _context.Items.Include(s=> s.SerialNumber)
                                            .Include(s=>s.Category)
                                            .ToListAsync();
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Categories"]= new SelectList(_context.Categories, "id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Categories"]= new SelectList(_context.Categories, "id", "Name");
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Categories"]= new SelectList(_context.Categories, "id", "Name");
            var item = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Categories"]= new SelectList(_context.Categories, "id", "Name");
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);
            return View(item);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
