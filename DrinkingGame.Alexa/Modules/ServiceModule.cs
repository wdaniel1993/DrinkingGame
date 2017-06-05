using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace DrinkingGame.WebService.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PuzzleService>().As<IPuzzleService>().InstancePerLifetimeScope();
            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
        }
    }
}