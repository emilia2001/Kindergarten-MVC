using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kindergarten.Models;
using Kindergarten.Data;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Kindergarten.Controllers
{
    [Authorize(Policy = "OnlyAdmin")]
    public class TeachersController : Controller
    {
        private readonly KindergartenContext _context;
        private string _baseUrl = "http://localhost:5043/api/Teachers";

        public TeachersController(KindergartenContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {

            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var teachers = JsonConvert.DeserializeObject<List<Teacher>>(await response.Content.ReadAsStringAsync());
                return View(teachers);
            }
            return NotFound();
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var teacher = JsonConvert.DeserializeObject<Teacher>(
                await response.Content.ReadAsStringAsync());
                return View(teacher);
            }
            return NotFound();
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherID,FirstName,LastName,ContactNumber")] Teacher teacher)
        {
            if (!ModelState.IsValid) return View(teacher);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(teacher);
                var response = await client.PostAsync(_baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message} ");
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var teacher = JsonConvert.DeserializeObject<Teacher>(
                await response.Content.ReadAsStringAsync());
                return View(teacher);
            }
            return new NotFoundResult();
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("TeacherID,FirstName,LastName,ContactNumber")] Teacher teacher)
        {
            if (!ModelState.IsValid)
                return View(teacher);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(teacher);
            var response = await client.PutAsync($"{_baseUrl}/{teacher.TeacherID}",
            new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var teacher = JsonConvert.DeserializeObject<Teacher>(await response.Content.ReadAsStringAsync());
                return View(teacher);
            }
            return new NotFoundResult();
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("TeacherID")] Teacher teacher)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{teacher.TeacherID}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(teacher), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message} ");
            }
            return View(teacher);
        }

        private bool TeacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.TeacherID == id)).GetValueOrDefault();
        }
    }
}
