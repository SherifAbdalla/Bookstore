using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SESSIONKEY = "Cart";
        public object BindModel(ControllerContext ControllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if(ControllerContext.HttpContext.Session != null)
            {
                cart = (Cart) ControllerContext.HttpContext.Session[SESSIONKEY];
            }
            if(cart == null)
            {
                cart = new Cart();
                if (ControllerContext.HttpContext.Session != null)
                    ControllerContext.HttpContext.Session[SESSIONKEY] = cart;
            }
            return cart;
        }
    }
}