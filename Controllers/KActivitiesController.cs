using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Kindergarten.Data;
using Kindergaten.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kindergarten.Controllers
{ 
    [Authorize(Policy = "OnlyStaff")]
    public class KActivitiesController : Controller
    {
        private readonly KindergartenContext _context;

        public KActivitiesController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: KActivities
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new ActivityClassIndexData();
            viewModel.Activities = await _context.KActivities
                          .Include(a => a.ClassActivities)
                          .ThenInclude(a => a.Class)
                          .ThenInclude(a => a.Teacher)
                          .OrderBy(a => a.Date)
                          .ToListAsync();

            if (id != null)
            {
                ViewData["KActivityID"] = id.Value;
                KActivity activity = viewModel.Activities.Where(a => a.KActivityID == id.Value).Single();
                viewModel.Classes = activity.ClassActivities.Select(activity => activity.Class);
            }
            return View(viewModel);
                          
        }

        // GET: KActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KActivities == null)
            {
                return NotFound();
            }

            var kActivity = await _context.KActivities
                .FirstOrDefaultAsync(m => m.KActivityID == id);
            if (kActivity == null)
            {
                return NotFound();
            }

            return View(kActivity);
        }

        // GET: KActivities/Create
        [Authorize(Policy = "OnlyAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: KActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([Bind("KActivityID,KActivityName,Date,Time")] KActivity kActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kActivity);
        }

        // GET: KActivities/Edit/5
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KActivities == null)
            {
                return NotFound();
            }

            var kActivity = await _context.KActivities
                .Include(a=> a.ClassActivities)
                .ThenInclude(a => a.Class)
                .ThenInclude(a => a.Teacher)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.KActivityID == id);
            if (kActivity == null)
            {
                return NotFound();
            }

            PopulateClassActivitiesData(kActivity);
            return View(kActivity);
        }

        private void PopulateClassActivitiesData(KActivity activity)
        {
            var allClasses = _context.Classes;
            var classActivities = new HashSet<int>(activity.ClassActivities.Select(c => c.ClassID));
            var viewModel = new List<EnrolledClassData>();

            foreach (var c in allClasses)
            {
                viewModel.Add(new EnrolledClassData
                {
                    ClassID = c.ClassID,
                    Name = c.ClassName,
                    Capacity = c.Capacity,
                    IsEnrolled = classActivities.Contains(c.ClassID)
                });
            }
            ViewData["Classes"] = viewModel;
        }

        // POST: KActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int id, string[] enrolledClasses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityToUpdate = await _context.KActivities
                .Include(a => a.ClassActivities)
                .ThenInclude(a => a.Class)
                .FirstOrDefaultAsync(a => a.KActivityID == id);

            if ( await TryUpdateModelAsync<KActivity> (activityToUpdate, "", a => a.KActivityName, a => a.Date, a => a.Time))
            {
                UpdateEnrolledClasses(enrolledClasses, activityToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }

            UpdateEnrolledClasses(enrolledClasses, activityToUpdate);
            PopulateClassActivitiesData(activityToUpdate);

            return View(activityToUpdate);

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(kActivity);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!KActivityExists(kActivity.KActivityID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(kActivity);
        }

        private void UpdateEnrolledClasses(string[] selectedClasses, KActivity activity)
        {
            if (selectedClasses == null)
            {
                activity.ClassActivities = new List<ClassActivity>();
                return;
            }

            var selected = new HashSet<string>(selectedClasses);
            var enrolled = new HashSet<int>(activity.ClassActivities.Select(c => c.Class.ClassID));

            foreach (var c in _context.Classes)
            {
                if (selected.Contains(c.ClassID.ToString()))
                {
                    if (!enrolled.Contains(c.ClassID))
                    {
                        activity.ClassActivities.Add(new ClassActivity
                        {
                            KActivityID = activity.KActivityID,
                            ClassID = c.ClassID
                        });
                    }
                }
                else
                {
                    if (enrolled.Contains(c.ClassID))
                    {
                        ClassActivity classToRemove = activity.ClassActivities.FirstOrDefault(i => i.ClassID == c.ClassID);
                        _context.Remove(classToRemove);
                    }
                }
            }
        }

        // GET: KActivities/Delete/5
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KActivities == null)
            {
                return NotFound();
            }

            var kActivity = await _context.KActivities
                .FirstOrDefaultAsync(m => m.KActivityID == id);
            if (kActivity == null)
            {
                return NotFound();
            }

            return View(kActivity);
        }

        // POST: KActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KActivities == null)
            {
                return Problem("Entity set 'KindergartenContext.KActivities'  is null.");
            }
            var kActivity = await _context.KActivities.FindAsync(id);
            if (kActivity != null)
            {
                _context.KActivities.Remove(kActivity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KActivityExists(int id)
        {
          return (_context.KActivities?.Any(e => e.KActivityID == id)).GetValueOrDefault();
        }
    }
}
