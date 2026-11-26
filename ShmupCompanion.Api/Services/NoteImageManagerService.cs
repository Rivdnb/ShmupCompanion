using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Interfaces;
using ShmupCompanion.Api.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ShmupCompanion.Api.Services
{
    public class NoteImageManagerService : INoteImageManagerService
    {
        private readonly ShmupCompanionContext _context;
        private readonly IImageStorageService _imageStorage;

        public NoteImageManagerService(ShmupCompanionContext context, IImageStorageService imageStorage)
        {
            _context = context;
            _imageStorage = imageStorage;
        }
        public List<int> ParseHtmlImageIds(string source)
        {
            List<int> imageIds = new List<int>();
            string? elementHtml;
            string? imageId;
            //Use the default configuration for AngleSharp
            AngleSharp.IConfiguration config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            IBrowsingContext context = BrowsingContext.New(config);

            IHtmlParser parser = context.GetService<IHtmlParser>()!;

            IDocument document = parser.ParseDocument(source);

            // Find list of noteImage id's from note html
            var imageElements = document.QuerySelectorAll<IHtmlElement>("img").ToList();
            foreach(var i in imageElements)
            {
                elementHtml = i.ToHtml();
                if (elementHtml == null) throw new Exception("noteImage html-element is null");

                // Finds first integer after "images/" (NOTE: routing might change)
                imageId = Regex.Match(elementHtml, @"(?<=images/)(\d+)").Value;
                if (imageId == null) throw new Exception("noteImage html-element missing an image Id");

                imageIds.Add(Convert.ToInt32(imageId));
            }
            return imageIds;
        }
        public void MarkNoteImagse(List<NoteImage> noteImages, bool toBeDeleted)
        {
            foreach (var noteImage in noteImages)
            {
                noteImage.ToBeDeleted = toBeDeleted;
                noteImage.MarkedForDeletionAt = toBeDeleted ? DateTime.UtcNow : null;
            }
        }
        public async Task UpdateNoteImagesAsync(int noteId)
        {
            // Get Note
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null) return;

            List<int> existingImageIds;

            try // Get noteImage id's of present images in Note
            { 
                existingImageIds = ParseHtmlImageIds(note.Text);
            }
            catch(Exception ex) // Error in parsing note's HTML, avoid (un)marking noteImages
            {
                Debug.WriteLine(ex.Message);
                return; 
            }
            // TODO-Optimize: This if-else can probably be combined/simplified
            if (existingImageIds.Count == 0) //unmark all noteImages, since none are present in note
            {
                var unmarkedNoteImages = await _context.NoteImages
                    .Where(noteImage => !noteImage.ToBeDeleted)
                    .Select(noteImage => noteImage)
                    .ToListAsync();

                // Mark these noteImages for deletion
                MarkNoteImagse(unmarkedNoteImages, true);
            }
            else
            {
                // Get noteImages present in Note that were marked for deletion
                var markedNoteImages = await _context.NoteImages
                    .Where(noteImage => noteImage.ToBeDeleted &&
                                        existingImageIds.Contains(noteImage.Id))
                    .Select(noteImage => noteImage)
                    .ToListAsync();

                // Unmark these noteImages
                MarkNoteImagse(markedNoteImages, false);

                // Get noteImages not present in Note, which are not yet marked for deletion
                var unmarkedNoteImages = await _context.NoteImages
                    .Where(noteImage => !noteImage.ToBeDeleted &&
                                        !existingImageIds.Contains(noteImage.Id))
                    .Select(noteImage => noteImage)
                    .ToListAsync();

                // Mark these noteImages for deletion
                MarkNoteImagse(unmarkedNoteImages, true);
            }
        }
        public async Task DeleteNoteImagesAsync(CancellationToken cancellationToken)
        {
            // Get note images that should be deleted
            var noteImages = await _context.NoteImages
                .Where(noteImage => noteImage.ToBeDeleted &&
                                    noteImage.MarkedForDeletionAt < DateTime.UtcNow.AddHours(-24))
                .Select(noteImage => noteImage)
                .ToListAsync(cancellationToken);

            if (noteImages == null) return;

            // Get image file paths
            List<string> filePaths = new List<string>();
            foreach (var noteImage in noteImages)
                filePaths.Add(noteImage.FilePath);

            // Delete note images from DB
            _context.NoteImages.RemoveRange(noteImages);
            await _context.SaveChangesAsync(cancellationToken);

            // Delete image files
            foreach (string filePath in filePaths)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _imageStorage.Delete(filePath); // TODO: needs error handling?
            }
        }
    }
}
