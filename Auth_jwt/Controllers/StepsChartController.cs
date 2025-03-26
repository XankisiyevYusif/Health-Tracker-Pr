using Auth_jwt.Data;
using Auth_jwt.Dtos;
using Auth_jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth_jwt.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StepsChartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _usermanager;
        private readonly ILogger<StepsChartController> _logger;

        public StepsChartController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager, ILogger<StepsChartController> logger)
        {
            _context = context;
            _usermanager = usermanager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StepData>>> GetWeeklySteps()
        {
            try
            {
                var userId = GetCurrentUserId();
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);

                var weeklySteps = await _context.StepData
                    .Where(s => s.UserId == userId && s.Date >= startOfWeek && s.Date <= endOfWeek)
                    .OrderBy(s => s.Date)
                    .ToListAsync();

                if (weeklySteps.Count == 0)
                {
                    var emptySteps = Enumerable.Range(0, 7)
                        .Select(i => new StepData
                        {
                            UserId = userId,
                            Date = startOfWeek.AddDays(i),
                            DayOfWeek = startOfWeek.AddDays(i).DayOfWeek.ToString(),
                            Steps = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        })
                        .ToList();

                    _context.StepData.AddRange(emptySteps);
                    await _context.SaveChangesAsync();

                    return Ok(emptySteps);
                }

                return Ok(weeklySteps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haftalık adım verisi alınırken hata oluştu");
                return StatusCode(500, "Haftalık adım verileri alınırken bir hata oluştu");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StepData>> AddSteps(StepDataDto stepDataDto)
        {
            try
            {
                if (stepDataDto.Steps < 0)
                {
                    return BadRequest("Adım sayısı negatif olamaz");
                }

                var userId = GetCurrentUserId();
                var today = DateTime.Today;

                var existingEntry = await _context.StepData
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.Date.Date == today);

                if (existingEntry != null)
                {
                    existingEntry.Steps = stepDataDto.Steps;
                    existingEntry.UpdatedAt = DateTime.UtcNow;

                    _context.StepData.Update(existingEntry);
                    await _context.SaveChangesAsync();

                    return Ok(existingEntry);
                }
                else
                {
                    var newStepData = new StepData
                    {
                        UserId = userId,
                        Date = today,
                        DayOfWeek = today.DayOfWeek.ToString(),
                        Steps = stepDataDto.Steps,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.StepData.Add(newStepData);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetWeeklySteps), new { id = newStepData.Id }, newStepData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Adım verisi eklenirken hata oluştu");
                return StatusCode(500, "Adım verisi kaydedilirken bir hata oluştu");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSteps(int id, StepDataDto stepDataDto)
        {
            try
            {
                if (stepDataDto.Steps < 0)
                {
                    return BadRequest("Step count cannot be negative");
                }

                var userId = GetCurrentUserId();

                var stepData = await _context.StepData
                    .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

                if (stepData == null)
                {
                    return NotFound();
                }

                stepData.Steps = stepDataDto.Steps;
                stepData.UpdatedAt = DateTime.UtcNow;

                _context.StepData.Update(stepData);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating step data");
                return StatusCode(500, "An error occurred while updating step data");
            }
        }

        private string GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new Exception("User ID not found");
            }
            return userId;
        }
    }
}
