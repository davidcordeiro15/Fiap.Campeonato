using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JogadoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/jogadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogador>>> GetJogadores()
        {
            return await _context.Jogadores
                .Include(j => j.Time)
                .ToListAsync();
        }

        // GET: api/jogadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogador>> GetJogador(int id)
        {
            var jogador = await _context.Jogadores
                .Include(j => j.Time)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jogador == null)
                return NotFound(new { erro = "Jogador não encontrado." });

            return jogador;
        }

        // POST: api/jogadores
        [HttpPost]
        public async Task<ActionResult<Jogador>> PostJogador(Jogador jogador)
        {
            var timeExiste = await _context.Times.AnyAsync(t => t.Id == jogador.TimeId);
            if (!timeExiste)
                return BadRequest(new { erro = "Time informado não existe." });

            _context.Jogadores.Add(jogador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJogador), new { id = jogador.Id }, jogador);
        }

        // PUT: api/jogadores/5 (trocar de time)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogador(int id, Jogador jogador)
        {
            if (id != jogador.Id)
                return BadRequest(new { erro = "ID inválido." });

            var timeExiste = await _context.Times.AnyAsync(t => t.Id == jogador.TimeId);
            if (!timeExiste)
                return BadRequest(new { erro = "Time informado não existe." });

            _context.Entry(jogador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Jogadores.Any(j => j.Id == id))
                    return NotFound(new { erro = "Jogador não encontrado." });
                throw;
            }

            return NoContent();
        }
    }
}
