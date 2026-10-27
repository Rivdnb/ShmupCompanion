using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts;

namespace ShmupCompanion.Api.Services
{
    public class GameService
    {
        private readonly ShmupCompanionContext _context;
        public GameService(ShmupCompanionContext context)
        {
            _context = context;
        }
        public async Task<List<GameDto>> GetAllAsync()
        {
            var games = await _context.Games
            .Select(game => new GameDto(
                game.Id,
                game.Name,
                game.Developer,
                game.ReleaseYear))
            .ToListAsync();

            return games;
        }

        public async Task<GameDto?> GetAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return null;
            return new GameDto(
                game.Id,
                game.Name,
                game.Developer,
                game.ReleaseYear);
        }

        public async Task<GameDto> AddAsync(GameRequest request)
        {
            Game game = new Game
            {
                Name = request.Name,
                Developer = request.Developer,
                ReleaseYear = request.ReleaseYear
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return new GameDto(
                game.Id,
                game.Name,
                game.Developer,
                game.ReleaseYear
            );
        }

        public async Task<bool> UpdateAsync(GameRequest request, int id)
        {
            var existing = await _context.Games.FindAsync(id);

            if (existing == null)
                return false;

            existing.Name = request.Name;
            existing.Developer = request.Developer;
            existing.ReleaseYear = request.ReleaseYear;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
                return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
