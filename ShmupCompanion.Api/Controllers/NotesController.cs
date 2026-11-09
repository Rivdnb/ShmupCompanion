using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Api.Services;
using ShmupCompanion.Contracts;
using static System.Net.Mime.MediaTypeNames;

namespace ShmupCompanion.Api.Controllers
{
    [ApiController]
    [Route("/api/games/{gameId:int}/runs/{runId:int}/notes")]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;
        public NotesController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAll(
            int gameId, int runId)
        {
            return await _noteService.GetAllForRunAsync(runId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> Get(
            int gameId, int runId, int id)
        {
            var note = await _noteService.GetAsync(runId, id);
            if (note == null) return NotFound();
            return note;
        }
        [HttpPost]
        public async Task<ActionResult<NoteDto>> Create(
            NoteRequest request, int gameId, int runId)
        {
            var note = await _noteService.AddAsync(request, runId);
            if (note == null) return NotFound();
            return CreatedAtAction(nameof(Get), new { gameId, runId, note.Id }, note);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            NoteRequest request, int gameId, int runId, int id)
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

        [HttpGet("{id}/images/{noteImageId:int}")]
        public async Task<IActionResult> GetImage(
            int gameId, int runId, int id, int noteImageId)
        {
            var noteImage = await _noteService.GetImageAsync(id, noteImageId);
            if (noteImage == null)
                return NotFound();

            return PhysicalFile(noteImage.FilePath, noteImage.ContentType);
        }

        [HttpPost("{id}/images")]
        public async Task<ActionResult<string>> CreateImage(
            int gameId, int runId, int id, IFormFile image)
        {;
            int noteImageId = await _noteService.AddImageAsync(id, image);
            if(noteImageId == -1) return NotFound();
            return Ok($"/api/games/{gameId}/runs/{runId}/notes/{id}/images/{noteImageId}");
        }
    }
}
