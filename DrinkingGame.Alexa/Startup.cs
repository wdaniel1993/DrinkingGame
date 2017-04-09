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
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            

            ConfigureDependency(app);

            
        }
    }
}
