using Autofac;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrappers.Autofac;
using Sriracha.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sriracha.Ioc
{
    public class NancyBootstrapper : AutofacNancyBootstrapper
    {
        private readonly IRootPathProvider _rootPathProvider;

        public NancyBootstrapper()
        {
            _rootPathProvider = null;
        }
        public NancyBootstrapper(IRootPathProvider rootPathProvider)
        {
            _rootPathProvider = rootPathProvider;
        }

        //http://stackoverflow.com/questions/17325840/registering-startup-class-in-nancy-using-autofac-bootstrapper
        protected override void ConfigureRequestContainer(ILifetimeScope container, Nancy.NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            
            var builder = new ContainerBuilder();
            builder.RegisterModule(new SrirachaAutofacModule(EnumIocMode.Web));
            builder.Register((x)=>new Sriracha.Data.Identity.Impl.WebSrirachaIdentity(context)).As<ISrirachaIdentity>();
            builder.Update(container.ComponentRegistry);
        }

        protected override void RequestStartup(ILifetimeScope container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>(),
            };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
        
        protected override IRootPathProvider RootPathProvider
        {
            get 
            { 
                return _rootPathProvider ?? base.RootPathProvider;
            }
        }
    }
}
