using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts.Games;
using SQLitePCL;
using System.Xml.Linq;

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

        public async Task AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Game game)
        {
            var existing = await _context.Games.FindAsync(game.Id);

            if (existing == null)
                return false;

            existing.Name = game.Name;

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
