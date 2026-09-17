using Microsoft.AspNetCore.Mvc;

namespace ShmupCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private static readonly string[] Games =
        [
            "Mushihimesama", "DoDonPachi", "Espgaluda II"
        ];

        private static readonly int[] ReleaseYears =
        [
            2004, 1997, 2005
        ];

        [HttpGet(Name = "GetGame")]
        public IEnumerable<Game> Get()
        {
            return Enumerable.Range(0, 3).Select(index => new Game
            {
                Name = Games[index],
                Id = index,
                Developer = "Cave",
                ReleaseYear = ReleaseYears[index]
            }).ToArray();
        }
    }
}
