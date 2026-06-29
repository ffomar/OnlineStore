using System.Security.Cryptography;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            var items = await _context.Items.Include(x=>x.Category)
                                            .Include(x=>x.Client)
                                            .ToListAsync();
            return View(items);
        }
    


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Clients = BuildClientSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item, string? clientKey)
        {
            ApplyClientKey(item, clientKey);

            // Generate a unique serial number for the item
            item.Serial = await GenerateUniqueSerialAsync();
            
            //reset the model state and validate the item again to include the generated serial number
            ModelState.Clear();
            TryValidateModel(item);

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                ViewBag.Clients = BuildClientSelectList(clientKey);
                return View(item);
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Edit(string serial)
        {
            var item = _context.Items.Find(serial);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Clients = BuildClientSelectList(GetClientKey(item.ClientName, item.ClientAddress));
            return View(item);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Item item, string? clientKey)
        {
            ApplyClientKey(item, clientKey);

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                ViewBag.Clients = BuildClientSelectList(clientKey);
                return View(item);
            }
            
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string serial)
        {
            var item = await _context.Items.FindAsync(serial);
            if (item == null)
            {
                return NotFound();
            }
            
            return View(item);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Item item)
        {
            var existingItem = await _context.Items.FindAsync(item.Serial);

            if (existingItem == null)
            {
                return NotFound();
            }

            _context.Items.Remove(existingItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }








        // Helper Method: Loops until an unused serial number is found
        private async Task<string> GenerateUniqueSerialAsync()
        {
            const string pool = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            bool isDuplicate = true;
            string finalSerial = string.Empty;

            // Keep looping until we find a serial number that does NOT exist in SQL Server
            while (isDuplicate)
            {
                var builder = new StringBuilder();
                for (int i = 0; i < 21; i++)
                {
                    if (i == 5 || i == 10 || i == 15)
                    {
                        builder.Append("-");
                    }

                    int randomIndex = RandomNumberGenerator.GetInt32(pool.Length);
                    builder.Append(pool[randomIndex]);
                }

                finalSerial = builder.ToString();

                // Check the database for collisions
                isDuplicate = await _context.Items.AnyAsync(x => x.Serial == finalSerial);
            }

            return finalSerial;
        }

        private SelectList BuildClientSelectList(string? selectedClientKey = null)
        {
            var clientOptions = _context.Clients
                .AsNoTracking()
                .Select(c => new
                {
                    CompositeKey = c.Name + "||" + c.Address,
                    DisplayText = c.Name + " - " + c.Address
                })
                .ToList();

            return new SelectList(clientOptions, "CompositeKey", "DisplayText", selectedClientKey);
        }

        private static string? GetClientKey(string? clientName, string? clientAddress)
        {
            if (string.IsNullOrWhiteSpace(clientName) || string.IsNullOrWhiteSpace(clientAddress))
            {
                return null;
            }

            return clientName + "||" + clientAddress;
        }

        private static void ApplyClientKey(Item item, string? clientKey)
        {
            if (string.IsNullOrWhiteSpace(clientKey))
            {
                item.ClientName = null;
                item.ClientAddress = null;
                return;
            }

            var parts = clientKey.Split("||", 2, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                item.ClientName = parts[0];
                item.ClientAddress = parts[1];
                return;
            }

            item.ClientName = null;
            item.ClientAddress = null;
        }

        [HttpPost]
        public async Task<IActionResult> ClearAll()
        {
            var allItems = await _context.Items.ToListAsync();
            _context.Items.RemoveRange(allItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
