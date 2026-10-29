using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts;
using ShmupCompanion.Api.Services;

namespace ShmupCompanion.Api.Controllers
{
    [ApiController]
    [Route("/games/{gameId:int}/runs/{runId:int}/notes")]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;
        public NotesController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAll(int gameId, int runId)
        {
            return await _noteService.GetAllForRunAsync(runId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> Get(int gameId, int runId, int id)
        {
            var note = await _noteService.GetAsync(runId, id);
            if (note == null) return NotFound();
            return note;
        }
        [HttpPost]
        public async Task<ActionResult<NoteDto>> Create(NoteRequest request, int gameId, int runId)
        {
            var note = await _noteService.AddAsync(request, runId);
            if (note == null) return NotFound();
            return CreatedAtAction(nameof(Get), new { gameId, runId, note.Id }, note);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(NoteRequest request, int gameId, int runId, int id)
        {
            bool updated = await _noteService.UpdateAsync(request, runId, id);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int gameId, int runId, int id)
        {
            bool deleted = await _noteService.DeleteAsync(runId, id);

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
