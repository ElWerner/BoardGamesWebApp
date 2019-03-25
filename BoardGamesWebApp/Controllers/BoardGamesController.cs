using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGamesWebApp.Context;
using BoardGamesWebApp.Models;
using BoardGamesWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesWebApp.Controllers
{
    /// <summary>
    /// Simple BoardGames controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardGamesController : Controller
    {
        private IBoardGameService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardGamesController"/> class
        /// </summary>
        /// <param name="service">BoardGames service</param>
        public BoardGamesController(IBoardGameService service)
        {
            _service = service;
            service.Initialize();
        }

        /// <summary>
        /// Gets list of board games
        /// </summary>
        /// <returns>Board games list</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<BoardGame>> GetAllBoardGames()
        {
            return await _service.GetAllBoardGamesAsync();
        }

        /// <summary>
        /// Gets a specific board game
        /// </summary>
        /// <param name="id">Board game id</param>
        /// <returns>Requested board game</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BoardGame>> GetBoardGameById(int id)
        {
            BoardGame boardGame = await _service.GetBoardGameByIdAsync(id);

            if (boardGame == null)
            {
                return NotFound();
            }

            return boardGame;
        }

        /// <summary>
        /// Creates new board game
        /// </summary>
        /// <param name="boardGame"></param>
        /// <returns>A newly created BoardGame</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BoardGame), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BoardGame>> AddBoardGame(BoardGame boardGame)
        {
            await _service.AddBoardGameAsync(boardGame);

            return CreatedAtAction(nameof(GetBoardGameById), new { id = boardGame.Id }, boardGame);
        }

        /// <summary>
        /// Edits specified board game
        /// </summary>
        /// <param name="id">Board game id</param>
        /// <param name="boardGame"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BoardGame), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditBoardGame(int id, BoardGame boardGame)
        {
            await _service.EditBoardGameAsync(id, boardGame);

            return NoContent();
        }

        /// <summary>
        /// Deletes specified board game
        /// </summary>
        /// <param name="id">Board game id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BoardGame), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBoardGameById(int id)
        {
            await _service.DeleteBoardGameAsync(id);

            return NoContent();
        }
    }
}