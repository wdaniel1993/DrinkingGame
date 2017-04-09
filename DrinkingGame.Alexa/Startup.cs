using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Autofac.Integration.SignalR;

[assembly: OwinStartup(typeof(DrinkingGame.Alexa.Startup))]

namespace DrinkingGame.Alexa
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            var httpConfiguration = new HttpConfiguration();
            var hubConfiguration = new HubConfiguration();

            WebApiConfig.Register(httpConfiguration);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfiguration);

            app.UseWebApi(httpConfiguration);
            app.MapSignalR(hubConfiguration);
        }
    }
}
