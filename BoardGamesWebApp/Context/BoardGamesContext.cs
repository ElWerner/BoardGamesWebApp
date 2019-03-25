using BoardGamesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamesWebApp.Context
{
    public class BoardGamesContext : DbContext
    {
        public BoardGamesContext(DbContextOptions<BoardGamesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BoardGame> BoardGames { get; set; }
    }
}
