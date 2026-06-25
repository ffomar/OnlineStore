using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyApp.Data;
using MyApp.Models;

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

        public IActionResult Edit(int id)
        {
            return Content("id = " + id);        
        }

        public async Task<IActionResult> Index()
        {
            var item = await _context.Items.ToListAsync();
            return View(item);
        }
    }
}
