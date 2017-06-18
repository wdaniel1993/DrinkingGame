using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace DrinkingGame.Client.Core.IoT
{
    public class AvoidanceSensorService : IAvoidanceSensorService
    {
        private readonly int _pinNumber;
        private GpioPin _avoidanceSensor;
        public IObservable<bool> Obstacle { get; }

        public AvoidanceSensorService(int pinNumber)
        {
            _pinNumber = pinNumber;
            InitGpio();

            if (_avoidanceSensor != null)
            {
                Obstacle = Observable.Interval(TimeSpan.FromMilliseconds(200))
                    .Select(_ => _avoidanceSensor.Read() == GpioPinValue.Low).DistinctUntilChanged();
            }
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                return;
            }
            _avoidanceSensor = gpio.OpenPin(_pinNumber);
            _avoidanceSensor.SetDriveMode(GpioPinDriveMode.Input);
        }

        public void Dispose()
        {
            _avoidanceSensor.Dispose();
        }
    }
}
