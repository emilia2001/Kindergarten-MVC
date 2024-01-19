using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kindergarten.Data;
using Microsoft.AspNetCore.Authorization;
using Kindergarten.Models;
using KindergatenModel;

namespace Kindergarten.Controllers
{
    [Authorize(Policy = "OnlyStaff")]
    public class ChildrenController : Controller
    {
        private readonly KindergartenContext _context;

        public ChildrenController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: Children
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber,
            int? classId)
        { 
            //var kindergartenContext = _context.Children.Include(c => c.Class);
            //return View(await kindergartenContext.ToListAsync());

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["Classes"]= await _context.Classes.ToListAsync();
            ViewData["SelectedClass"] = classId;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var children = from c in _context.Children.Where(c => !classId.HasValue || c.ClassID == classId)
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                children = children.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    children = children.OrderByDescending(b => b.LastName);
                    break;
                default:
                    children = children.OrderBy(b => b.LastName);
                    break;
            }
            int pageSize = 4;
            return View(await PaginatedList<Child>.CreateAsync(children.Include(b => b.Class).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Children/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Children == null)
            {
                return NotFound();
            }

            var child = await _context.Children
                .Include(c => c.Class)
                .FirstOrDefaultAsync(m => m.ChildID == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // GET: Children/Create
        [Authorize(Policy = "OnlyAdmin")]
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([Bind("ChildID,FirstName,LastName,DateOfBirth,ParentContact,ClassID")] Child child)
        {
            if (ModelState.IsValid)
            {
                _context.Add(child);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", child.ClassID);
            return View(child);
        }

        // GET: Children/Edit/5
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Children == null)
            {
                return NotFound();
            }

            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", child.ClassID);
            return View(child);
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ChildID,FirstName,LastName,DateOfBirth,ParentContact,ClassID")] Child child)
        {
            if (id != child.ChildID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(child);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChildExists(child.ChildID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", child.ClassID);
            return View(child);
        }

        // GET: Children/Delete/5
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Children == null)
            {
                return NotFound();
            }

            var child = await _context.Children
                .Include(c => c.Class)
                .FirstOrDefaultAsync(m => m.ChildID == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Children == null)
            {
                return Problem("Entity set 'KindergartenContext.Children'  is null.");
            }
            var child = await _context.Children.FindAsync(id);
            if (child != null)
            {
                _context.Children.Remove(child);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChildExists(int id)
        {
          return (_context.Children?.Any(e => e.ChildID == id)).GetValueOrDefault();
        }
    }
}
