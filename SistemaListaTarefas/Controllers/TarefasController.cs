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

       // 1. LISTAGEM PRINCIPAL [cite: 17, 18]
        public async Task<IActionResult> Index()
        {
           // Regra: Ordenar pelo campo Ordem [cite: 21]
            var tarefas = await _context.Tarefas.OrderBy(t => t.Ordem).ToListAsync();
            return View(tarefas);
        }

        // 2. INCLUIR (GET) [cite: 39]
        public IActionResult Create()
        {
            return View();
        }

        // 2. INCLUIR (POST) [cite: 40]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                // Regra: Validar nome duplicado [cite: 11, 44]
                if (_context.Tarefas.Any(t => t.Nome == tarefa.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe uma tarefa com este nome.");
                    return View(tarefa);
                }

               // Regra: Nova tarefa deve ser a última da lista [cite: 43]
                int novaOrdem = _context.Tarefas.Any() ? _context.Tarefas.Max(t => t.Ordem) + 1 : 1;
                tarefa.Ordem = novaOrdem;

                _context.Add(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tarefa);
        }

        // 3. EDITAR (GET) [cite: 29, 38]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            // Retorna a Partial View para o Modal
            return PartialView("_EditPartial", tarefa);
        }

        // 3. EDITAR (POST) [cite: 30]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Verifica duplicidade antes de salvar [cite: 33, 34]
                bool nomeJaExiste = await _context.Tarefas.AnyAsync(t => t.Nome == tarefa.Nome && t.Id != id);
                if (nomeJaExiste)
                {
                    ModelState.AddModelError("Nome", "Já existe uma tarefa com este nome.");
                    return PartialView("_EditPartial", tarefa); // Retorna o erro dentro do modal
                }

                _context.Update(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Atualiza a página principal 
            }
            return PartialView("_EditPartial", tarefa);
        }

        // 4. EXCLUIR [cite: 26, 27]
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

        // 5. REORDENAÇÃO - SUBIR [cite: 45, 51]
        public async Task<IActionResult> Subir(int id)
        {
            var atual = await _context.Tarefas.FindAsync(id);
            if (atual == null) return NotFound();

            var anterior = await _context.Tarefas
                .Where(t => t.Ordem < atual.Ordem)
                .OrderByDescending(t => t.Ordem)
                .FirstOrDefaultAsync();

            if (anterior != null)
            {
                int temp = atual.Ordem;
                atual.Ordem = anterior.Ordem;
                anterior.Ordem = temp;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

       // 5. REORDENAÇÃO - DESCER [cite: 45, 51]
        public async Task<IActionResult> Descer(int id)
        {
            var atual = await _context.Tarefas.FindAsync(id);
            if (atual == null) return NotFound();

            var proxima = await _context.Tarefas
                .Where(t => t.Ordem > atual.Ordem)
                .OrderBy(t => t.Ordem)
                .FirstOrDefaultAsync();

            if (proxima != null)
            {
                int temp = atual.Ordem;
                atual.Ordem = proxima.Ordem;
                proxima.Ordem = temp;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}