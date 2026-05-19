using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartidasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/partidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPartidas()
        {
            return await _context.Partidas
                .Include(p => p.TimeCasa)
                .Include(p => p.TimeVisitante)
                .Include(p => p.Campeonato)
                .Select(p => new
                {
                    p.Id,
                    TimeCasa = p.TimeCasa!.Nome,
                    TimeVisitante = p.TimeVisitante!.Nome,
                    Campeonato = p.Campeonato!.Nome,
                    p.DataPartida,
                    p.GolsCasa,
                    p.GolsVisitante,
                    Resultado = p.GolsCasa == null ? "Aguardando" :
                                p.GolsCasa > p.GolsVisitante ? $"{p.TimeCasa.Nome} venceu" :
                                p.GolsCasa < p.GolsVisitante ? $"{p.TimeVisitante.Nome} venceu" : "Empate"
                })
                .ToListAsync();
        }

        // GET: api/partidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partida>> GetPartida(int id)
        {
            var partida = await _context.Partidas
                .Include(p => p.TimeCasa)
                .Include(p => p.TimeVisitante)
                .Include(p => p.Campeonato)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partida == null)
                return NotFound(new { erro = "Partida não encontrada." });

            return partida;
        }

        // POST: api/partidas  — registra resultado de uma partida com gols
        [HttpPost]
        public async Task<ActionResult<Partida>> PostPartida(Partida partida)
        {
            if (partida.TimeCasaId == partida.TimeVisitanteId)
                return BadRequest(new { erro = "Um time não pode jogar contra si mesmo." });

            var timeCasaExiste = await _context.Times.AnyAsync(t => t.Id == partida.TimeCasaId);
            var timeVisitanteExiste = await _context.Times.AnyAsync(t => t.Id == partida.TimeVisitanteId);
            var campeonatoExiste = await _context.Campeonatos.AnyAsync(c => c.Id == partida.CampeonatoId);

            if (!timeCasaExiste || !timeVisitanteExiste)
                return BadRequest(new { erro = "Um ou mais times informados não existem." });

            if (!campeonatoExiste)
                return BadRequest(new { erro = "Campeonato informado não existe." });

            _context.Partidas.Add(partida);
            await _context.SaveChangesAsync();

            // Se o resultado já vem preenchido, atualiza estatísticas
            if (partida.GolsCasa.HasValue && partida.GolsVisitante.HasValue)
                await AtualizarEstatisticas(partida);

            return CreatedAtAction(nameof(GetPartida), new { id = partida.Id }, partida);
        }

        // PUT: api/partidas/{id}/resultado — registra/atualiza o placar de uma partida existente
        [HttpPut("{id}/resultado")]
        public async Task<IActionResult> RegistrarResultado(int id, [FromBody] ResultadoDto resultado)
        {
            var partida = await _context.Partidas.FindAsync(id);
            if (partida == null)
                return NotFound(new { erro = "Partida não encontrada." });

            if (resultado.GolsCasa < 0 || resultado.GolsVisitante < 0)
                return BadRequest(new { erro = "Gols não podem ser negativos." });

            partida.GolsCasa = resultado.GolsCasa;
            partida.GolsVisitante = resultado.GolsVisitante;

            await _context.SaveChangesAsync();
            await AtualizarEstatisticas(partida);

            return Ok(new
            {
                mensagem = "Resultado registrado e classificação atualizada.",
                partida.GolsCasa,
                partida.GolsVisitante
            });
        }

        private async Task AtualizarEstatisticas(Partida partida)
        {
            var golsCasa = partida.GolsCasa!.Value;
            var golsVisitante = partida.GolsVisitante!.Value;

            var statCasa = await ObterOuCriarEstatistica(partida.TimeCasaId, partida.CampeonatoId);
            var statVisitante = await ObterOuCriarEstatistica(partida.TimeVisitanteId, partida.CampeonatoId);

            statCasa.GolsPro += golsCasa;
            statCasa.GolsContra += golsVisitante;

            statVisitante.GolsPro += golsVisitante;
            statVisitante.GolsContra += golsCasa;

            if (golsCasa > golsVisitante)
            {
                // Casa venceu
                statCasa.Vitorias++;
                statCasa.Pontos += 3;
                statVisitante.Derrotas++;
            }
            else if (golsVisitante > golsCasa)
            {
                // Visitante venceu
                statVisitante.Vitorias++;
                statVisitante.Pontos += 3;
                statCasa.Derrotas++;
            }
            else
            {
                // Empate
                statCasa.Empates++;
                statCasa.Pontos++;
                statVisitante.Empates++;
                statVisitante.Pontos++;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Estatistica> ObterOuCriarEstatistica(int timeId, int campeonatoId)
        {
            var stat = await _context.Estatisticas
                .FirstOrDefaultAsync(e => e.TimeId == timeId && e.CampeonatoId == campeonatoId);

            if (stat == null)
            {
                stat = new Estatistica { TimeId = timeId, CampeonatoId = campeonatoId };
                _context.Estatisticas.Add(stat);
                await _context.SaveChangesAsync();
            }

            return stat;
        }
    }

    public class ResultadoDto
    {
        public int GolsCasa { get; set; }
        public int GolsVisitante { get; set; }
    }
}
