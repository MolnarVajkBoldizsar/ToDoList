using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ListController : Controller
    {
        private readonly ToDoListContext _context;

        public ListController(ToDoListContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Lists.ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Date")] List list)
        {
            if(ModelState.IsValid)
            {
                _context.Lists.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(list);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == id);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Date")] List list)
        {
            if(ModelState.IsValid)
            {
                _context.Update(list);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(list);
        }

        /*public async Task<IActionResult> Delete(int id)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == id);
            return View(list);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if(list != null)
            {
                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction("Index", "Lists");

        }*/

        // Rename one of the Delete methods to resolve the CS0111 error.
        // Here, the second Delete method is renamed to DeleteConfirmed to differentiate it.

        public async Task<IActionResult> Delete(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null) return NotFound();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Renamed method
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null) return NotFound();

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
    }
}
