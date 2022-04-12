using System;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext: DbContext
    {

        private static string ConnectionString = @"Server=;
                                                   User Id=;
                                                   Password=;
                                                   Database=;
                                                   MultipleActiveResultSets=true";
            
        
        public DbSet<BoardSquare> BoardSquares { get; set; } = default!;
        public DbSet<GameConfig> GameConfigs { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Ship> Ships { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !e.IsOwned())
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        
    }
}