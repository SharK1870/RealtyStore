using System.Collections.Generic;
using System.Linq;

namespace RealtyStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Realty realty, int quantity)
        {
            CartLine line = lineCollection
                .Where(r => r.Realty.RealtyId == realty.RealtyId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Realty = realty,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Realty realty)
        {
            lineCollection.RemoveAll(l => l.Realty.RealtyId == realty.RealtyId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Realty.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Realty Realty { get; set; }
        public int Quantity { get; set; }
    }
}
