using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Dialogs;
using DrinkingGame.WebService.Handler;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Luis;

namespace DrinkingGame.WebService.Modules
{
    public class CommunicatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new LuisModelAttribute("82ff336f-f639-4866-b416-7761d4a8f126", "b0c3c0efe8444b97adf92cce9e31997c")).AsSelf().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<LuisService>().As<ILuisService>().SingleInstance();
            builder.RegisterType<DrinkingGameCommunicator>().As<IDrinkingGameCommunicator>().InstancePerLifetimeScope();
            builder.RegisterType<GameHandler>().As<IStartable>().SingleInstance();
            builder.RegisterType<GameDialog>().AsSelf().InstancePerDependency();
        }
    }
}