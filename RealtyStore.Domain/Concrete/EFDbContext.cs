using RealtyStore.Domain.Entities;
using System.Data.Entity;

namespace RealtyStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Realty> Realties { get; set; }
    }
}
