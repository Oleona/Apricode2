using Apricode4.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Apricode4.Models
{
    public class VideoGameShopContext : DbContext
    {
        public VideoGameShopContext(DbContextOptions<VideoGameShopContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public VideoGameShopContext()
        {
        }

        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }

}
