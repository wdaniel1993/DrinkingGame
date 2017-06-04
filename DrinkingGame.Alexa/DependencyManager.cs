using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace DrinkingGame.WebService
{
    public class DependencyManager
    {

        public IContainer Container { get; set; }
        private static DependencyManager _instance;

        private DependencyManager()
        { }

        public static DependencyManager Current => _instance ?? (_instance = new DependencyManager());

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}