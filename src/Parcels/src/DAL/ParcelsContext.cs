using Microsoft.EntityFrameworkCore;
using Parcels.EventEntities;

namespace Parcels.DAL
{
    public class ParcelsContext : DbContext
    {
        public ParcelsContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<ParcelProjection> Parcels { get; set; }
        public DbSet<BaseEvent> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEvent>()
                .Ignore(x => x.IsApplied);

            modelBuilder.Entity<BaseEvent>()
                .Property(x => x.Sequence)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BaseEvent>()
                .Property(x => x.Version)
                .ValueGeneratedNever();
        }
    }
}
