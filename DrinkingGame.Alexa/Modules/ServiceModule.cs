using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using DrinkingGame.Alexa.Communicators;
using DrinkingGame.Alexa.Services;

namespace DrinkingGame.Alexa.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
        }
    }
}