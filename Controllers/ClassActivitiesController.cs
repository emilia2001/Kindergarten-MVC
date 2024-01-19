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
    public class ClassActivitiesController : Controller
    {
        private readonly KindergartenContext _context;

        public ClassActivitiesController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: ClassActivities
        public async Task<IActionResult> Index()
        {
            var kindergartenContext = _context.ClassesActivities.Include(c => c.Class).Include(c => c.KActivity);
            return View(await kindergartenContext.ToListAsync());
        }

        // GET: ClassActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassesActivities == null)
            {
                return NotFound();
            }

            var classActivity = await _context.ClassesActivities
                .Include(c => c.Class)
                .Include(c => c.KActivity)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classActivity == null)
            {
                return NotFound();
            }

            return View(classActivity);
        }

        // GET: ClassActivities/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            ViewData["KActivityID"] = new SelectList(_context.KActivities, "KActivityID", "KActivityName");
            return View();
        }

        // POST: ClassActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassID,KActivityID")] ClassActivity classActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classActivity.ClassID);
            ViewData["KActivityID"] = new SelectList(_context.KActivities, "KActivityID", "KActivityName", classActivity.KActivityID);
            return View(classActivity);
        }

        // GET: ClassActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassesActivities == null)
            {
                return NotFound();
            }

            var classActivity = await _context.ClassesActivities.FindAsync(id);
            if (classActivity == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classActivity.ClassID);
            ViewData["KActivityID"] = new SelectList(_context.KActivities, "KActivityID", "KActivityName", classActivity.KActivityID);
            return View(classActivity);
        }

        // POST: ClassActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassID,KActivityID")] ClassActivity classActivity)
        {
            if (id != classActivity.ClassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassActivityExists(classActivity.ClassID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", classActivity.ClassID);
            ViewData["KActivityID"] = new SelectList(_context.KActivities, "KActivityID", "KActivityName", classActivity.KActivityID);
            return View(classActivity);
        }

        // GET: ClassActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassesActivities == null)
            {
                return NotFound();
            }

            var classActivity = await _context.ClassesActivities
                .Include(c => c.Class)
                .Include(c => c.KActivity)
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classActivity == null)
            {
                return NotFound();
            }

            return View(classActivity);
        }

        // POST: ClassActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassesActivities == null)
            {
                return Problem("Entity set 'KindergartenContext.ClassesActivities'  is null.");
            }
            var classActivity = await _context.ClassesActivities.FindAsync(id);
            if (classActivity != null)
            {
                _context.ClassesActivities.Remove(classActivity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassActivityExists(int id)
        {
          return (_context.ClassesActivities?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }
    }
}
