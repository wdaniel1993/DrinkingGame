using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Dialogs;
using DrinkingGame.WebService.Handler;
using DrinkingGame.WebService.Services;

namespace DrinkingGame.WebService.Modules
{
    public class CommunicatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DrinkingGameCommunicator>().As<IDrinkingGameCommunicator>().InstancePerLifetimeScope();
            builder.RegisterType<GameHandler>().As<IStartable>().SingleInstance();
            builder.RegisterType<GameDialog>().AsSelf().InstancePerDependency();
        }
    }
}