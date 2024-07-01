using Microsoft.EntityFrameworkCore;
using Projekt_server.Entities;

namespace Projekt_server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Server> Servers { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Taskk> Tasks { get; set; }
    }
}
