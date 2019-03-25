using BoardGamesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamesWebApp.Services
{
    public interface IBoardGameService
    {
        Task<IEnumerable<BoardGame>> GetAllBoardGamesAsync();

        Task<BoardGame> GetBoardGameByIdAsync(int id);

        Task AddBoardGameAsync(BoardGame boardGame);

        Task<BoardGame> EditBoardGameAsync(int id, BoardGame boardGame);

        Task DeleteBoardGameAsync(int id);

        void Initialize();
    }
}
