using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace Bookstore.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        public DbSet<Book> Books { get; }
    }
}
