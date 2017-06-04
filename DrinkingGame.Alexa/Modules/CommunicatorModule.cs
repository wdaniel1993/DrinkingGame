using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Handler;

namespace DrinkingGame.WebService.Modules
{
    public class CommunicatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DrinkingGameCommunicator>().As<IDrinkingGameCommunicator>().InstancePerLifetimeScope();
            builder.RegisterType<GameHandler>().As<IStartable>().SingleInstance();
        }
    }
}