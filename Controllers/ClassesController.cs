using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kindergarten.Controllers
{
    [Authorize(Policy = "OnlyStaff")]
    public class ClassesController : Controller
    {
        private readonly KindergartenContext _context;

        public ClassesController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var kindergartenContext = _context.Classes.Include(c => c.Teacher);
            return View(await kindergartenContext.ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Classes/Create
        [Authorize(Policy = "OnlyAdmin")]
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "FullName");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([Bind("ClassID,ClassName,TeacherID,Capacity")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "FullName", @class.TeacherID);
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "FullName", @class.TeacherID);
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClassID,ClassName,TeacherID,Capacity")] Class @class)
        {
            if (id != @class.ClassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.ClassID))
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
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "FullName", @class.TeacherID);
            return View(@class);
        }

        // GET: Classes/Delete/5
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'KindergartenContext.Classes'  is null.");
            }
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
          return (_context.Classes?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }
    }
}
