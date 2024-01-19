using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kindergarten.Models;
using Kindergarten.Data;
using Microsoft.AspNetCore.Authorization;
using Kindergaten.Models.ViewModel;
using System.Globalization;
using System.Drawing;

namespace Kindergarten.Controllers
{
    [Authorize(Policy = "OnlyStaff")]
    public class AttendancesController : Controller
    {
        private readonly KindergartenContext _context;

        public AttendancesController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index(int? year, string? month, int? classId)
        {
            int selectedMonthIndex = -1;
            if (!string.IsNullOrEmpty(month))
            {
                // Find the index of the selected month in the month names array
                selectedMonthIndex = Array.IndexOf(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames, month) + 1;
            }
            // If year and month parameters are not provided, use the current month and year
            var currentYear = year ?? DateTime.Now.Year;
            var currentMonth = selectedMonthIndex != -1 ? selectedMonthIndex : DateTime.Now.Month;

            var children = await _context.Children.Include(c => c.Attendances).ToListAsync();

            var viewModelList = children
                .Where(c => !classId.HasValue || c.ClassID == classId)
                .Select(c => new ChildAttendanceViewModel
            {
                ChildID = c.ChildID,
                FullName = c.FullName,
                TotalAttendancesThisMonth = c.Attendances.Count(a => a.Date.Month == currentMonth && a.Date.Year == currentYear)
            }).OrderBy(c => c.FullName).ToList();

            ViewBag.Months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            ViewBag.SelectedYear = currentYear;
            ViewBag.SelectedMonth = month;
            ViewBag.Classes = await _context.Classes.ToListAsync();
            ViewBag.SelectedClass = classId;

            return View(viewModelList);
        }




    // GET: Attendances/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Child)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["ChildID"] = new SelectList(_context.Children, "ChildID", "FirstName");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceID,ChildID,Date,PresentStatus")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChildID"] = new SelectList(_context.Children, "ChildID", "FirstName", attendance.ChildID);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        [Authorize(Policy = "OnlyTeacher")]
        public async Task<IActionResult> Edit(int? classId)
        {
            var children = await _context.Children.Where(c => !classId.HasValue || c.ClassID == classId).OrderBy(c => c.LastName).Include(c => c.Attendances).ToListAsync();
            ViewBag.Classes = await _context.Classes.ToListAsync();
            ViewBag.SelectedClass = classId;
            //var viewModelList = children.Select(c => new ChildAttendanceViewModel
            //{
            //    ChildID = c.ChildID,
            //    FullName = c.FullName,
            //    TotalAttendancesThisMonth = c.Attendances.Count(a => a.Date.Month == currentMonth && a.Date.Year == currentYear)
            //}).ToList();

            return View(children);
        }


        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyTeacher")]
        public async Task<IActionResult> Edit(List<int> childIds)
        {
            // Validate and process the submitted data
            if (ModelState.IsValid)
            {
                // Get the current date
                DateTime currentDate = DateTime.Now.Date;

                // Loop through the childIds and update attendance
                foreach (var childId in childIds)
                {
                    var child = await _context.Children.Include(c => c.Attendances)
                                                       .FirstOrDefaultAsync(c => c.ChildID == childId);

                    if (child != null)
                    {
                        var attendance = child.Attendances.FirstOrDefault(a => a.Date.Date == currentDate);
                        if (attendance != null)
                        {
                            // Update the PresentStatus based on checkbox
                            attendance.PresentStatus = childIds.Contains(childId);
                        }
                        else
                        {
                            // If attendance record for the current date doesn't exist, create a new one
                            child.Attendances.Add(new Attendance { Date = currentDate, PresentStatus = childIds.Contains(childId) });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, redisplay the form
            return View();
        }


        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Child)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendances == null)
            {
                return Problem("Entity set 'KindergartenContext.Attendances'  is null.");
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
          return (_context.Attendances?.Any(e => e.AttendanceID == id)).GetValueOrDefault();
        }
    }
}
