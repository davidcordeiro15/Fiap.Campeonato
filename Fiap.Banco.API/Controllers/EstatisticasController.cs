using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatisticasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstatisticasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/estatisticas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEstatisticas()
        {
            return await _context.Estatisticas
                .Include(e => e.Time)
                .Include(e => e.Campeonato)
                .OrderBy(e => e.Campeonato!.Nome)
                .ThenByDescending(e => e.Pontos)
                .Select(e => new
                {
                    Time = e.Time!.Nome,
                    Campeonato = e.Campeonato!.Nome,
                    e.Pontos,
                    e.Vitorias,
                    e.Empates,
                    e.Derrotas,
                    e.GolsPro,
                    e.GolsContra,
                    SaldoGols = e.GolsPro - e.GolsContra
                })
                .ToListAsync();
        }

        // GET: api/estatisticas/campeonato/{campeonatoId}
        [HttpGet("campeonato/{campeonatoId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetEstatisticasPorCampeonato(int campeonatoId)
        {
            var campeonatoExiste = await _context.Campeonatos.AnyAsync(c => c.Id == campeonatoId);
            if (!campeonatoExiste)
                return NotFound(new { erro = "Campeonato não encontrado." });

            return await _context.Estatisticas
                .Where(e => e.CampeonatoId == campeonatoId)
                .Include(e => e.Time)
                .OrderByDescending(e => e.Pontos)
                .ThenByDescending(e => e.Vitorias)
                .ThenByDescending(e => e.GolsPro - e.GolsContra)
                .Select(e => new
                {
                    Posicao = 0, // calculado client-side ou via window function
                    Time = e.Time!.Nome,
                    e.Pontos,
                    e.Vitorias,
                    e.Empates,
                    e.Derrotas,
                    e.GolsPro,
                    e.GolsContra,
                    SaldoGols = e.GolsPro - e.GolsContra
                })
                .ToListAsync();
        }
    }
}
