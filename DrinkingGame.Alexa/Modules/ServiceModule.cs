using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Services;

namespace DrinkingGame.WebService.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
        }
    }
}