using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DrinkingGame.Shared.Interfaces;
using DrinkingGame.WebService.Handler;
using DrinkingGame.WebService.Hubs;
using DrinkingGame.WebService.Modules;

namespace DrinkingGame.WebService
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

            builder.Register(ctx => hubConfiguration.Resolver.Resolve<IConnectionManager>().GetHubContext<DrinkingGameHub, IGameClient>())
                .ExternallyOwned();

            var container = builder.Build();

            DependencyManager.Current.Container = container;
            
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            hubConfiguration.Resolver = new AutofacDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfiguration);

            app.UseWebApi(httpConfiguration);
            app.MapSignalR(hubConfiguration);

            container.Resolve<DrinkingGameHub>();
        }
    }
}