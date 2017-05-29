using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkingGame.WebService.Handler
{
    public interface IGameHandler : IDisposable
    {
        void StartHandler();

        bool IsActive { get; }
    }
}
