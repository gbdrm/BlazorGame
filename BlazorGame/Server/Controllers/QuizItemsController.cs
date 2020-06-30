using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlazorGame.Data;
using BlazorGame.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlazorGame.Server.Controllers
{
    [Authorize]
    public class QuizItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuizItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuizItems.ToListAsync());
        }

        // GET: QuizItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizItem = await _context.QuizItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizItem == null)
            {
                return NotFound();
            }

            return View(quizItem);
        }

        // GET: QuizItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuizItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer,ExperiencePoints")] QuizItem quizItem)
        {
            if (ModelState.IsValid)
            {
                quizItem.Id = Guid.NewGuid();
                _context.Add(quizItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quizItem);
        }

        // GET: QuizItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizItem = await _context.QuizItems.FindAsync(id);
            if (quizItem == null)
            {
                return NotFound();
            }
            return View(quizItem);
        }

        // POST: QuizItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Question,Answer,ExperiencePoints")] QuizItem quizItem)
        {
            if (id != quizItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizItemExists(quizItem.Id))
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
            return View(quizItem);
        }

        // GET: QuizItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizItem = await _context.QuizItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizItem == null)
            {
                return NotFound();
            }

            return View(quizItem);
        }

        // POST: QuizItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var quizItem = await _context.QuizItems.FindAsync(id);
            _context.QuizItems.Remove(quizItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizItemExists(Guid id)
        {
            return _context.QuizItems.Any(e => e.Id == id);
        }
    }
}
