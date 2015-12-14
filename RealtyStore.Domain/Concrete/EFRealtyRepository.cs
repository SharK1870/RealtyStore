using System.Collections.Generic;
using RealtyStore.Domain.Entities;
using RealtyStore.Domain.Abstract;

namespace RealtyStore.Domain.Concrete
{
    public class EFRealtyRepository : IRealtyRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Realty> Realties
        {
            get { return context.Realties; }
        }

        public void SaveRealty(Realty realty)
        {
            if (realty.RealtyId == 0)
            {
                context.Realties.Add(realty);
            }
            else
            {
                Realty dbEntry = context.Realties.Find(realty.RealtyId);
                if (dbEntry != null)
                {
                    dbEntry.Name = realty.Name;
                    dbEntry.Description = realty.Description;
                    dbEntry.Price = realty.Price;
                    dbEntry.Category = realty.Category;
                    //dbEntry.ImageData = realty.ImageData;
                    //dbEntry.ImageMineType = realty.ImageMineType;
                }
            }
            context.SaveChanges();
        }

        public Realty DeleteRealty(int realtyId)
        {
            Realty dbEntry = context.Realties.Find(realtyId);
            if (dbEntry != null)
            {
                context.Realties.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
