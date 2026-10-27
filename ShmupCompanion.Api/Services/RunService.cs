using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Data;
using ShmupCompanion.Api.Models;
using ShmupCompanion.Contracts;

namespace ShmupCompanion.Api.Services
{
    public class RunService
    {
        private readonly ShmupCompanionContext _context;
        public RunService(ShmupCompanionContext context)
        {
            _context = context;
        }
        public async Task<List<RunDto>> GetAllForGameAsync(int gameId)
        {
            var runs = await _context.Runs
            .Where(run => run.GameId == gameId)
            .Select(run => new RunDto(
                run.Id,
                run.Name,
                run.GameId
            ))
            .ToListAsync();

            return runs;
        }

        public async Task<RunDto?> GetAsync(int gameId, int id)
        {
            var run = await _context.Runs.FindAsync(id);

            if (run == null) return null;
            if (run.GameId !=  gameId) return null; // TODO error: routed game doesn't match the game belonging to run

            return new RunDto(
                run.Id,
                run.Name,
                run.GameId
            );
        }

        public async Task<RunDto?> AddAsync(RunRequest request, int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) return null; // TODO implement proper error handling later

            Run run = new Run
            {
                Name = request.Name,
                GameId = gameId,
                Game = game
            };
            _context.Runs.Add(run);
            await _context.SaveChangesAsync();
            return new RunDto(
                run.Id,
                run.Name,
                run.GameId
            );
        }

        public async Task<bool> UpdateAsync(RunRequest request, int gameId, int id)
        {
            var existing = await _context.Runs.FindAsync(id);

            if (existing == null) return false;
            if (existing.GameId != gameId) return false; // TODO error: routed game doesn't match the game belonging to run

            existing.Name = request.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int gameId, int id)
        {
            var run = await _context.Runs.FindAsync(id);

            if (run == null) return false;
            if (run.GameId != gameId) return false; // TODO error: routed game doesn't match the game belonging to run

            _context.Runs.Remove(run);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
