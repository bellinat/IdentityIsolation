using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.WebApi;
using EP.IdentityIsolation.Infra.CrossCutting.IoC;
using EP.IdentityIsolation.API.App_Start;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace EP.IdentityIsolation.API.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            // Chamada dos módulos do Simple Injector
            InitializeContainer(container);

            // Necessário para registrar o ambiente do Owin que é dependência do Identity
            // Feito fora da camada de IoC para não levar o System.Web para fora
            container.RegisterWebApiRequest(() =>
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["owin.Environment"] == null && container.IsVerifying())
                {
                    return new OwinContext().Authentication;
                }
                return HttpContext.Current.GetOwinContext().Authentication;
            });

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.RegisterWebApiRequest<ISecureDataFormat<AuthenticationTicket>, SecureDataFormat<AuthenticationTicket>>();
            container.RegisterWebApiRequest<ITextEncoder, Base64UrlTextEncoder>();
            container.RegisterWebApiRequest<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            container.RegisterWebApiRequest<IDataProtector>(() => new DpapiDataProtectionProvider("Eduardo").Create("ASP.NET Identity"));

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            BootStrapper.RegisterServices(container);
        }
    }
}