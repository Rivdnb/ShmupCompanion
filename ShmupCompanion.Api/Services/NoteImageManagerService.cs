using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Interfaces;
using ShmupCompanion.Api.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ShmupCompanion.Api.Services
{
    public class NoteImageManagerService : INoteImageManagingService
    {
        private readonly ShmupCompanionContext _context;

        public NoteImageManagerService(ShmupCompanionContext context)
        {
            _context = context;
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

        public async Task UpdateNoteImagesAsync(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null) return;

            List<int> imageIds = ParseHtmlImageIds(note.Text);
            if(imageIds.Count == 0) return;

            // TODO: update noteImage info for this note

        }
    }
}
