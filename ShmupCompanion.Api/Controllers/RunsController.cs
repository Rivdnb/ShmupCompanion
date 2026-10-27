using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts;
using ShmupCompanion.Api.Services;

namespace ShmupCompanion.Api.Controllers
{
    [ApiController]
    [Route("/games/{gameId:int}/runs")]
    public class RunsController : ControllerBase
    {
        private readonly RunService _runService;
        public RunsController(RunService runService)
        {
            _runService = runService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RunDto>>> GetAll(int gameId)
        {
            return await _runService.GetAllForGameAsync(gameId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<RunDto>> Get(int gameId, int id)
        {
            var run = await _runService.GetAsync(gameId, id);
            if (run == null) return NotFound();
            return run;
        }
        [HttpPost]
        public async Task<ActionResult<RunDto>> Create(RunRequest request, int gameId)
        {
            var run = await _runService.AddAsync(request, gameId);
            if (run == null) return NotFound();
            return CreatedAtAction(nameof(Get), new { gameId, run.Id }, run);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(RunRequest request, int gameId, int id)
        {
            bool updated = await _runService.UpdateAsync(request, gameId, id);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int gameId, int id)
        {
            bool deleted = await _runService.DeleteAsync(gameId, id);

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
