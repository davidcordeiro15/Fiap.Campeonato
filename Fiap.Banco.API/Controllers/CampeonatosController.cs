using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampeonatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CampeonatosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/campeonatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campeonato>>> GetCampeonatos()
        {
            return await _context.Campeonatos
                .Include(c => c.Times)
                    .ThenInclude(tc => tc.Time)
                .ToListAsync();
        }

        // GET: api/campeonatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Campeonato>> GetCampeonato(int id)
        {
            var campeonato = await _context.Campeonatos
                .Include(c => c.Times)
                    .ThenInclude(tc => tc.Time)
                .Include(c => c.Partidas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campeonato == null)
                return NotFound(new { erro = "Campeonato não encontrado." });

            return campeonato;
        }

        // POST: api/campeonatos
        [HttpPost]
        public async Task<ActionResult<Campeonato>> PostCampeonato(CampeonatoPontosCorridos campeonato)
        {
            _context.CampeonatosPontosCorridos.Add(campeonato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampeonato), new { id = campeonato.Id }, campeonato);
        }

        // POST: api/campeonatos/{id}/times/{timeId}
        [HttpPost("{id}/times/{timeId}")]
        public async Task<IActionResult> AdicionarTime(int id, int timeId)
        {
            var campeonato = await _context.Campeonatos.FindAsync(id);
            if (campeonato == null)
                return NotFound(new { erro = "Campeonato não encontrado." });

            var time = await _context.Times.FindAsync(timeId);
            if (time == null)
                return NotFound(new { erro = "Time não encontrado." });

            var jaExiste = await _context.TimesCampeonatos
                .AnyAsync(tc => tc.CampeonatoId == id && tc.TimeId == timeId);

            if (jaExiste)
                return BadRequest(new { erro = "Time já está inscrito neste campeonato." });

            _context.TimesCampeonatos.Add(new TimeCampeonato
            {
                TimeId = timeId,
                CampeonatoId = id
            });

            // Cria estatística zerada para o time no campeonato
            _context.Estatisticas.Add(new Estatistica
            {
                TimeId = timeId,
                CampeonatoId = id
            });

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Time '{time.Nome}' adicionado ao campeonato '{campeonato.Nome}'." });
        }

        // POST: api/campeonatos/{id}/gerarconfrontos
        [HttpPost("{id}/gerarconfrontos")]
        public async Task<IActionResult> GerarConfrontos(int id)
        {
            var campeonato = await _context.CampeonatosPontosCorridos
                .Include(c => c.Times)
                    .ThenInclude(tc => tc.Time)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campeonato == null)
                return NotFound(new { erro = "Campeonato não encontrado." });

            if (campeonato.ConfrontosGerados)
                return BadRequest(new { erro = "Confrontos já foram gerados para este campeonato." });

            var times = campeonato.Times.Select(tc => tc.Time!).ToList();

            if (times.Count < 2)
                return BadRequest(new { erro = "São necessários pelo menos 2 times para gerar os confrontos." });

            var partidas = new List<Partida>();

            // Todos contra todos (ida e volta)
            for (int i = 0; i < times.Count; i++)
            {
                for (int j = 0; j < times.Count; j++)
                {
                    if (i == j) continue; // não joga contra si mesmo

                    partidas.Add(new Partida
                    {
                        TimeCasaId = times[i].Id,
                        TimeVisitanteId = times[j].Id,
                        CampeonatoId = id,
                        DataPartida = campeonato.DataInicio.AddDays(partidas.Count * 7)
                    });
                }
            }

            _context.Partidas.AddRange(partidas);

            campeonato.ConfrontosGerados = true;
            campeonato.TotalRodadas = times.Count - 1;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Confrontos gerados com sucesso!",
                totalPartidas = partidas.Count,
                times = times.Select(t => t.Nome)
            });
        }

        // GET: api/campeonatos/{id}/classificacao
        [HttpGet("{id}/classificacao")]
        public async Task<IActionResult> GetClassificacao(int id)
        {
            var campeonatoExiste = await _context.Campeonatos.AnyAsync(c => c.Id == id);
            if (!campeonatoExiste)
                return NotFound(new { erro = "Campeonato não encontrado." });

            var classificacao = await _context.Estatisticas
                .Where(e => e.CampeonatoId == id)
                .Include(e => e.Time)
                .OrderByDescending(e => e.Pontos)
                .ThenByDescending(e => e.Vitorias)
                .ThenByDescending(e => e.GolsPro - e.GolsContra)
                .Select(e => new
                {
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

            return Ok(classificacao);
        }
    }
}
