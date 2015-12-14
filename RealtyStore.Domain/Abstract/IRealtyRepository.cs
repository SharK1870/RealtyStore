using System.Collections.Generic;
using RealtyStore.Domain.Entities;

namespace RealtyStore.Domain.Abstract
{
    public interface IRealtyRepository
    {
        IEnumerable<Realty> Realties { get; }

        void SaveRealty(Realty realty);

        Realty DeleteRealty(int realtyId);
    }
}
