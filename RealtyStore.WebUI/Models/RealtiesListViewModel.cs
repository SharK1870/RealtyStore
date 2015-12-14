using System.Collections.Generic;
using RealtyStore.Domain.Entities;

namespace RealtyStore.WebUI.Models
{
    public class RealtiesListViewModel
    {
        public IEnumerable<Realty> Realties { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}