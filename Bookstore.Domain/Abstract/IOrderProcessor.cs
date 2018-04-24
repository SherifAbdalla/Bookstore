using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Adstract
{
    public interface IOrderProcessor
    {
        void ProcessorOrder(Cart cart, ShippingDetails ShippingDetails);
    }
}
