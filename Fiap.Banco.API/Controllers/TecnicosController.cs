using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TecnicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TecnicosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tecnicos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tecnico>>> GetTecnicos()
        {
            return await _context.Tecnicos
                .Include(t => t.Time)
                .ToListAsync();
        }

        // GET: api/tecnicos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tecnico>> GetTecnico(int id)
        {
            var tecnico = await _context.Tecnicos
                .Include(t => t.Time)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tecnico == null)
                return NotFound(new { erro = "Técnico não encontrado." });

            return tecnico;
        }

        // POST: api/tecnicos
        [HttpPost]
        public async Task<ActionResult<Tecnico>> PostTecnico(Tecnico tecnico)
        {
            var timeExiste = await _context.Times.AnyAsync(t => t.Id == tecnico.TimeId);
            if (!timeExiste)
                return BadRequest(new { erro = "Time informado não existe." });

            _context.Tecnicos.Add(tecnico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTecnico), new { id = tecnico.Id }, tecnico);
        }
    }
}
