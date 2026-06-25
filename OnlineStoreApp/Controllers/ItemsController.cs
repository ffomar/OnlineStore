using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
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
            var item = await _context.Items.ToListAsync();
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
