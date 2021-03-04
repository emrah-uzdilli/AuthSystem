using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthSystem.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AuthSystem.Controllers
{

    [Authorize]
    public class OgrencilerController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly IHostingEnvironment _environment;

     
        public OgrencilerController(AuthDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Ogrenciler

      
        public ViewResult Index(string searchString)
        {
            
            var students = from s in _context.Ogrenciler
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.adi.ToUpper().Contains(searchString.ToUpper())
                                       || s.soyadi.ToUpper().Contains(searchString.ToUpper()));
            }
      
            return View(students.ToList());
        }

        // GET: Ogrenciler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        // GET: Ogrenciler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ogrenciler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,adi,soyadi,adres,Ulkesi,ResimDosyası")] Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ogrenci);

                string resimler = Path.Combine(_environment.WebRootPath, "resimler");
                if (ogrenci.ResimDosyası.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(resimler, ogrenci.ResimDosyası.FileName), FileMode.Create))
                    {
                        await ogrenci.ResimDosyası.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    System.Console.WriteLine("Null Exception Error!!");
                }
         
                ogrenci.ResimYolu = ogrenci.ResimDosyası.FileName;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ogrenci);
        }

        // GET: Ogrenciler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            return View(ogrenci);
        }

        // POST: Ogrenciler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,adi,soyadi,adres,Ulkesi")] Ogrenci ogrenci)
        {
            if (id != ogrenci.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogrenci);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgrenciExists(ogrenci.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ogrenci);
        }

        // GET: Ogrenciler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        // POST: Ogrenciler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OgrenciExists(int id)
        {
            return _context.Ogrenciler.Any(e => e.ID == id);
        }
    }
}
