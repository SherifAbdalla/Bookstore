using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Domain.Book book, int quantity = 1)
        {
            CartLine line = lineCollection
                            .Where(b => b.Book.BookID == book.BookID)
                            .FirstOrDefault();
            if(line == null)
            {
                lineCollection.Add(new CartLine { Book = book, Quantity = quantity });
            }else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Domain.Book book)
        {
            lineCollection.RemoveAll(b => b.Book.BookID == book.BookID);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(cl => cl.Book.Price * cl.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }
        }
        
    }
    public class CartLine
    {
        public Domain.Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
