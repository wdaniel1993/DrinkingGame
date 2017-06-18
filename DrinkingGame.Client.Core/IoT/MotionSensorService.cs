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
    public class MotionSensorService : IMotionSensorService
    {
        private readonly int _pinNumber;
        private GpioPin _motionSensor;
        public IObservable<bool> Obstacle { get; }

        public MotionSensorService(int pinNumber)
        {
            _pinNumber = pinNumber;
            InitGpio();

            if (_motionSensor != null)
            {
                Obstacle = Observable.Interval(TimeSpan.FromMilliseconds(200))
                    .Select(_ => _motionSensor.Read() == GpioPinValue.Low).DistinctUntilChanged();
            }
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                return;
            }
            _motionSensor = gpio.OpenPin(_pinNumber);
            _motionSensor.SetDriveMode(GpioPinDriveMode.Input);
        }

        public void Dispose()
        {
            _motionSensor.Dispose();
        }
    }
}
