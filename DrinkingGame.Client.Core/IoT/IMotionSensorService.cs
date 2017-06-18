using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkingGame.Client.Core.IoT
{
    public interface IMotionSensorService : IDisposable
    {
        IObservable<bool> Obstacle { get; }
    }
}
