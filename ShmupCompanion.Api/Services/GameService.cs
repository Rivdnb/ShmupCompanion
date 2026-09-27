using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Models;
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
        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetAsync(int id)
        {
            return await _context.Games.FindAsync(id);
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
