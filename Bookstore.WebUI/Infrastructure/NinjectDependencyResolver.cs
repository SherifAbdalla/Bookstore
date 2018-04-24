using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Adstract;
using Bookstore.Domain.Concrete;
using Bookstore.WebUI.Infrastructure.Adstract;
using Bookstore.WebUI.Infrastructure.Concrete;

namespace Bookstore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel Kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            Kernel = kernelParam;
            AddBindings();
        }
        public void AddBindings()
        {   EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "False"),

            };        
            Kernel.Bind<IBookRepository>().To< EFBookRepository>();

            Kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("setting", emailSettings);
            Kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }
    }
}