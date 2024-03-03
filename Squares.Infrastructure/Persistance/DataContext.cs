using Microsoft.EntityFrameworkCore;
using Squares.Domain.Entities;

namespace Squares.Infrastructure.Persistance
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<PointsList> PointsLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}