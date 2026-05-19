using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiap.Banco.API.Data;
using Fiap.Banco.API.Models;

namespace Fiap.Banco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/times
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Time>>> GetTimes()
        {
            return await _context.Times.ToListAsync();
        }

        // GET: api/times/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Time>> GetTime(int id)
        {
            var time = await _context.Times.FindAsync(id);
            if (time == null)
                return NotFound(new { erro = "Time não encontrado." });

            return time;
        }

        // POST: api/times
        [HttpPost]
        public async Task<ActionResult<Time>> PostTime(Time time)
        {
            // 🥚 Easter Egg: Palmeiras não tem mundial
            if (time.Nome.Trim().Equals("Palmeiras", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { erro = "Proibido times sem mundial!!!" });

            _context.Times.Add(time);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTime), new { id = time.Id }, time);
        }

        // PUT: api/times/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTime(int id, Time time)
        {
            if (id != time.Id)
                return BadRequest(new { erro = "ID da rota não corresponde ao ID do body." });

            // 🥚 Easter Egg também no update
            if (time.Nome.Trim().Equals("Palmeiras", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { erro = "Proibido times sem mundial!!!" });

            _context.Entry(time).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Times.Any(t => t.Id == id))
                    return NotFound(new { erro = "Time não encontrado." });
                throw;
            }

            return NoContent();
        }

        // DELETE: api/times/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTime(int id)
        {
            var time = await _context.Times.FindAsync(id);
            if (time == null)
                return NotFound(new { erro = "Time não encontrado." });

            _context.Times.Remove(time);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
