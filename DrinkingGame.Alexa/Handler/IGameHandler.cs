using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace DrinkingGame.WebService.Handler
{
    public interface IGameHandler : IDisposable, IStartable
    {
        bool IsActive { get; }
    }
}
