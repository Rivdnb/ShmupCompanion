using ShmupCompanion.Api.Interfaces;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace ShmupCompanion.Api.Services
{
    public class NoteImageManagerService : INoteImageManagingService
    {
        public List<int> ParseHtmlImageIds(int noteId)
        {
            //Use the default configuration for AngleSharp
            AngleSharp.IConfiguration config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            IBrowsingContext context = BrowsingContext.New(config);

            IHtmlParser parser = context.GetService<IHtmlParser>()!;

            // change this to note content
            var source = "<h1>Some example source</h1><p>This is a paragraph element";
            IDocument document = parser.ParseDocument(source);

            //find list of noteImage id's from document

            return [0]; // TODO
        }

        public void UpdateNoteImages(int noteId)
        {
            return; // TODO
        }
    }
}
