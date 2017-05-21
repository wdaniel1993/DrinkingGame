using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using DrinkingGame.Alexa.Modules;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace DrinkingGame.Alexa
{
    public partial class Startup
    {
        public void ConfigureDependency(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            var hubConfiguration = new HubConfiguration();

            WebApiConfig.Register(httpConfiguration);

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<CommunicatorModule>();

            builder.Register(_ => hubConfiguration.Resolver.Resolve<IConnectionManager>());

            var container = builder.Build();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            hubConfiguration.Resolver = new AutofacDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfiguration);

            app.UseWebApi(httpConfiguration);
            app.MapSignalR(hubConfiguration);
        }
    }
}