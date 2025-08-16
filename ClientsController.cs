using ClientAdmin.Data;
using ClientAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientAdmin.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;
        public ClientsController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string? busca)
        {
            var q = _context.Clientes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(busca))
                q = q.Where(c => EF.Functions.Like(c.Nome, $"%{busca}%"));
            ViewBag.Busca = busca;
            var list = await q.OrderBy(c => c.Nome).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new Cliente { DataCadastro = DateTime.Today });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (cliente.DataNascimento.Date > DateTime.Today)
                ModelState.AddModelError(nameof(Cliente.DataNascimento), "A data não pode ser futura.");

            cliente.CPF = new string((cliente.CPF ?? "").Where(char.IsDigit).ToArray());

            if (await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF))
                ModelState.AddModelError(nameof(Cliente.CPF), "CPF já cadastrado.");

            if (!ModelState.IsValid) return View(cliente);

            cliente.DataCadastro = DateTime.Today;
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (cliente.DataNascimento.Date > DateTime.Today)
                ModelState.AddModelError(nameof(Cliente.DataNascimento), "A data não pode ser futura.");

            cliente.CPF = new string((cliente.CPF ?? "").Where(char.IsDigit).ToArray());

            if (await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF && c.Id != id))
                ModelState.AddModelError(nameof(Cliente.CPF), "CPF já cadastrado.");

            if (!ModelState.IsValid) return View(cliente);

            try
            {
                _context.Entry(cliente).Property(c => c.DataCadastro).IsModified = false;
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Clientes.AnyAsync(c => c.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }
    }
}
