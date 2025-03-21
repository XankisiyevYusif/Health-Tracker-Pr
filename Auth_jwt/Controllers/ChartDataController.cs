using Auth_jwt.Data;
using Auth_jwt.Dtos;
using Auth_jwt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth_jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ChartDataController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChartData>>> GetChartData()
        {
            return await _context.ChartData.OrderBy(c => c.Date).ToListAsync();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChartData(int id, ChartData chartData)
        {
            if (id != chartData.Id)
            {
                return BadRequest();
            }
            _context.Entry(chartData).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChartDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<ChartData>> AddData([FromBody] ChartDataDto request)
        {
            if (request == null || request.Value <= 0)
            {
                return BadRequest("Invalid value.");
            }

            var newData = new ChartData { Value = request.Value, Date = DateTime.UtcNow }; // Add a date if needed
            _context.ChartData.Add(newData);
            await _context.SaveChangesAsync();

            return Ok(newData);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChartData(int id)
        {
            var chartData = await _context.ChartData.FindAsync(id);
            if (chartData == null)
            {
                return NotFound();
            }
            _context.ChartData.Remove(chartData);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool ChartDataExists(int id)
        {
            return _context.ChartData.Any(e => e.Id == id);
        }
    }
}
