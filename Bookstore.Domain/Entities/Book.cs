using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bookstore.Domain.Entities
{
    public class Book
    {
        
        public int BookID { get; set; }
        public string Title { get; set; }
        
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Specialization { get; set; }
        public string Author { get; set; }
    }
}
