using Autofac;
using DrinkingGame.Alexa.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkingGame.Alexa.Modules
{
    public class CommunicatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<DrinkingGameCommunicator>().As<IDrinkingGameCommunicator>().InstancePerLifetimeScope();
        }
    }
}