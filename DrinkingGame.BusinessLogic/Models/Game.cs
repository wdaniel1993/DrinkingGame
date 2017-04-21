using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Game
    {
        private readonly List<Player> _players = new List<Player>();
        private readonly List<Device> _devices = new List<Device>();

        private readonly Subject<Player> _playerAdded = new Subject<Player>();
        private readonly Subject<Device> _deviceAdded = new Subject<Device>();

        public int Id { get; }

        public IEnumerable<Player> Players => _players.AsReadOnly();
        public IEnumerable<Device> Devices => _devices.AsReadOnly();

        public GameStateMachine Machine { get; }

        public Game(int id)
        {
            Id = id;
            Machine = new GameStateMachine(new StateFactory(this));
        }

        public IObservable<Player> PlayerAdded => _playerAdded.AsObservable();

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            _playerAdded.OnNext(player);
        }

        public void AddDevice(Device device)
        {
            _devices.Add(device);
            _deviceAdded.OnNext(device);
        }
    }
}
