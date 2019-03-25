using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGamesWebApp.Context;
using BoardGamesWebApp.Exceptions;
using BoardGamesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BoardGamesWebApp.Services
{
    public class BoardGameService : IBoardGameService
    {
        private readonly BoardGamesContext _context;
        private readonly IMemoryCache _cache;

        public BoardGameService(BoardGamesContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void Initialize()
        {
            if (!_context.BoardGames.Any())
            {
                _context.BoardGames.AddRange(
                    new BoardGame { Name = "UNO", Description = "A shedding-type card game that is played with a specially printed deck." },
                    new BoardGame { Name = "Monopoly", Description = "A classic fast-dealing property trading board game." },
                    new BoardGame { Name = "Munchkin", Description = "A satirical card game based on the clichés and oddities of Dungeons and Dragons and other role-playing games." }
                );
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<BoardGame>> GetAllBoardGamesAsync()
        {
            return await _context.BoardGames.ToListAsync();
        }

        public async Task<BoardGame> GetBoardGameByIdAsync(int id)
        {
            BoardGame boardGame = null;

            if (!_cache.TryGetValue(id, out boardGame))
            {
                boardGame = await _context.BoardGames.FirstOrDefaultAsync(g => g.Id == id);

                if (boardGame != null)
                {
                    _cache.Set(boardGame.Id, boardGame, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                }
            }

            return boardGame;
        }

        public async Task AddBoardGameAsync(BoardGame boardGame)
        {
            await _context.BoardGames.AddAsync(boardGame);

            await _context.SaveChangesAsync();

            _cache.Set(boardGame.Id, boardGame, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        public async Task<BoardGame> EditBoardGameAsync(int id, BoardGame boardGame)
        {
            var game = await _context.BoardGames.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                throw new RequestedResourceNotFoundException();
            }

            _context.Entry(boardGame).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _cache.Set(boardGame.Id, boardGame, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return boardGame;
        }

        public async Task DeleteBoardGameAsync(int id)
        {
            var boardGame = await _context.BoardGames.FirstOrDefaultAsync(g => g.Id == id);

            if (boardGame == null)
            {
                throw new RequestedResourceNotFoundException();
            }

            _context.BoardGames.Remove(boardGame);

            await _context.SaveChangesAsync();

            if (_cache.TryGetValue(id, out var deletedBoardGame))
            {
                 _cache.Remove(boardGame.Id);
            }
        }

    }
}
