using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Interfaces;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts;
using System.Net.Mime;


namespace ShmupCompanion.Api.Services
{
    public class NoteService
    {
        private readonly ShmupCompanionContext _context;

        private readonly IImageStorageService _imageStorage;
        public NoteService(
            ShmupCompanionContext context, 
            IImageStorageService imageStorage)
        {
            _context = context;
            _imageStorage = imageStorage;
        }
        public async Task<List<NoteDto>> GetAllForRunAsync(int runId)
        {
            var notes = await _context.Notes
            .Where(note => note.RunId == runId)
            .Select(note => new NoteDto(
                note.Id,
                note.Name,
                note.RunId,
                note.Text
            ))
            .ToListAsync();

            return notes;
        }

        public async Task<NoteDto?> GetAsync(int runId, int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null) return null;
            if (note.RunId !=  runId) return null; // TODO error: routed run doesn't match the run belonging to note

            return new NoteDto(
                note.Id,
                note.Name,
                note.RunId,
                note.Text
            );
        }

        public async Task<NoteDto?> AddAsync(NoteRequest request, int runId)
        {
            var run = await _context.Runs.FindAsync(runId);
            if (run == null) return null; // TODO implement proper error handling later

            Note note = new Note
            {
                Name = request.Name,
                RunId = runId,
                Run = run,
                Text = request.Text
            };
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return new NoteDto(
                note.Id,
                note.Name,
                note.RunId,
                note.Text
            );
        }

        public async Task<bool> UpdateAsync(NoteRequest request, int runId, int id)
        {
            var existing = await _context.Notes.FindAsync(id);

            if (existing == null) return false;
            if (existing.RunId != runId) return false; // TODO error: routed run doesn't match the run belonging to note

            existing.Name = request.Name;
            existing.Text = request.Text;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int runId, int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null) return false;
            if (note.RunId != runId) return false; // TODO error: routed run doesn't match the run belonging to note

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<NoteImageDto?> GetImageAsync(int noteId, int noteImageId)
        {
            var noteImage = await _context.NoteImages.
                FirstOrDefaultAsync(i => i.Id == noteImageId && i.NoteId == noteId);

            if (noteImage == null) return null;

            string filePath = _imageStorage.GetFilePath(noteImage.FilePath);

            return new NoteImageDto(
                noteImage.Id,
                noteImage.NoteId,
                noteImage.FileName,
                noteImage.ContentType,
                filePath
            );
        }
        public async Task<int> AddImageAsync(int noteId, IFormFile image)
        { // TODO: implement and check
            var note = await _context.Notes
                .FindAsync(noteId);

            if (note == null)
                return -1;

            // Validate file

            string fileName = await _imageStorage.SaveAsync(image); // (GUID).(extension)

            var noteImage = new NoteImage
            {
                NoteId = noteId,
                FileName = image.FileName, // original filename
                ContentType = image.ContentType,
                FilePath = fileName // GUID filename
            };

            _context.NoteImages.Add(noteImage);

            await _context.SaveChangesAsync();

            return noteImage.Id;
        }
    }
}
