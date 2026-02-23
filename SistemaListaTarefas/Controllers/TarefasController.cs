using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaListaTarefas.Data;
using SistemaListaTarefas.Models;

namespace SistemaListaTarefas.Controllers
{
    public class TarefasController : Controller
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefas.OrderBy(t => t.Ordem).ToListAsync();
            return View(tarefas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                if (_context.Tarefas.Any(t => t.Nome == tarefa.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe uma tarefa com este nome.");
                    return View(tarefa);
                }

                int novaOrdem = _context.Tarefas.Any() ? _context.Tarefas.Max(t => t.Ordem) + 1 : 1;
                tarefa.Ordem = novaOrdem;

                _context.Add(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tarefa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            return PartialView("_EditPartial", tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                bool nomeJaExiste = await _context.Tarefas.AnyAsync(t => t.Nome == tarefa.Nome && t.Id != id);
                if (nomeJaExiste)
                {
                    ModelState.AddModelError("Nome", "Já existe uma tarefa com este nome.");
                    return PartialView("_EditPartial", tarefa);
                }

                _context.Update(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_EditPartial", tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reordenar([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0) return BadRequest();

            try
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    var tarefa = await _context.Tarefas.FindAsync(ids[i]);
                    if (tarefa != null)
                    {
                        tarefa.Ordem = i + 1;
                        _context.Update(tarefa);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}