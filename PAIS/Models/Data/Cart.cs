using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Book book, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Book.BookID == book.BookID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Book book)
            => lineCollection.RemoveAll(l => l.Book.BookID == book.BookID);

        public virtual decimal ComputeTotalValue()
            => lineCollection.Sum(e => e.Book.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
