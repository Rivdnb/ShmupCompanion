using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts.Game;
using ShmupCompanion.Api.Services;

namespace ShmupCompanion.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;
        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAll()
        {
            return await _gameService.GetAllAsync();
        }   
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> Get(int id)
        {
            var game = await _gameService.GetAsync(id);
            if (game == null) return NotFound();
            return game;
        }
        [HttpPost]
        public async Task<ActionResult<GameDto>> Create(GameRequest request)
        {
            var game = await _gameService.AddAsync(request);
            return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(GameRequest request, int id)
        {
            bool updated = await _gameService.UpdateAsync(request, id);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _gameService.DeleteAsync(id);

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
