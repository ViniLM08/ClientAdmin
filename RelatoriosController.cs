using ClientAdmin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientAdmin.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _context;
        public RelatoriosController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string periodo = "mes")
        {
            DateTime hoje = DateTime.Today;
            DateTime inicio;
            periodo = periodo?.ToLowerInvariant() ?? "mes";

            switch (periodo)
            {
                case "hoje":
                    inicio = hoje;
                    break;
                case "semana":
                    int diff = (7 + (int)hoje.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                    inicio = hoje.AddDays(-diff);
                    break;
                default:
                    inicio = new DateTime(hoje.Year, hoje.Month, 1);
                    periodo = "mes";
                    break;
            }

            var baseQuery = _context.Clientes
                .Where(c => c.DataCadastro >= inicio && c.DataCadastro <= hoje);

            // Busca apenas clientes com renda para cálculo em memória
            var clientesComRenda = await baseQuery
                .Where(c => c.RendaFamiliar.HasValue)
                .ToListAsync();

            // Média calculada no cliente para evitar erro do SQLite
            var mediaRenda = clientesComRenda
                .Average(c => c.RendaFamiliar.Value);

            // Contagem calculada no cliente para evitar problemas de tipo e tradução
            int maiores18RendaMaiorQueMedia = clientesComRenda
                .Count(c =>
                    c.RendaFamiliar.Value > mediaRenda &&
                    (hoje.Year - c.DataNascimento.Year -
                        (hoje < c.DataNascimento.AddYears(hoje.Year - c.DataNascimento.Year) ? 1 : 0)) >= 18);

            // Essas contagens podem continuar no banco
            int classeA = await baseQuery.CountAsync(c => c.RendaFamiliar.HasValue && c.RendaFamiliar <= 980m);
            int classeB = await baseQuery.CountAsync(c => c.RendaFamiliar.HasValue && c.RendaFamiliar > 980m && c.RendaFamiliar <= 2500m);
            int classeC = await baseQuery.CountAsync(c => c.RendaFamiliar.HasValue && c.RendaFamiliar > 2500m);

            ViewBag.Periodo = periodo;
            ViewBag.MediaRenda = mediaRenda;
            ViewBag.MaiorIdadeERendaAcimaMedia = maiores18RendaMaiorQueMedia;
            ViewBag.ClasseA = classeA;
            ViewBag.ClasseB = classeB;
            ViewBag.ClasseC = classeC;

            return View();
        }
    }
}
